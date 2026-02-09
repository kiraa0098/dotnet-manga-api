using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MANGA_DOMAIN.Abstractions.Interfaces;
using MANGA_DOMAIN.Entities;
using MANGA_INFRASTRUCTURE.External.MangaDex;

namespace MANGA_INFRASTRUCTURE.External.MangaDex
{
    public class MangaDexClient : IMangaSource
    {
        private readonly HttpClient _httpClient;
        private readonly MangaDexOptions _options;
        private readonly ILogger<MangaDexClient> _logger;
        public MangaDexClient(HttpClient httpClient, IOptions<MangaDexOptions> options, ILogger<MangaDexClient> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        #region GetByIdAsync
        public async Task<IReadOnlyList<MangaSourceEntity>> GetByIdAsync(string id, CancellationToken ct)
        {
            try
            {
                var url = $"{_options.BaseUrl}/manga/{id}";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.UserAgent.ParseAdd(_options.UserAgent);
                var response = await _httpClient.SendAsync(request, ct);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("MangaDex API returned: {StatusCode} - {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
                    return new List<MangaSourceEntity>();
                }

                var json = await response.Content.ReadAsStringAsync(ct);
                var doc = JsonDocument.Parse(json);
                var manga = doc.RootElement.GetProperty("data");
                var mangaId = manga.GetProperty("id").GetString();
                var mangaTitleObj = manga.GetProperty("attributes").GetProperty("title");
                var mangaTitle = mangaTitleObj.TryGetProperty("en", out var en) ? en.GetString() : null;
                var description = manga.GetProperty("attributes").TryGetProperty("description", out var descObj) && descObj.TryGetProperty("en", out var descEn) ? descEn.GetString() : null;

                // TODO: CoverUrl and Chapters
                return new List<MangaSourceEntity> { new MangaSourceEntity(mangaId ?? string.Empty, mangaTitle ?? string.Empty, description, null) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling MangaDex API for details");
                return new List<MangaSourceEntity>();
            }
        }
        #endregion


        #region SearchAsync
        public async Task<IReadOnlyList<MangaSourceEntity>> SearchAsync(string title, int pageNumber, CancellationToken ct)
        {
            try
            {
                int offset = MANGA_DOMAIN.Enums.MangaPagingConstants.GetOffset(pageNumber);

                var url = _options.BuildSearchUrl(title, offset);
                _logger.LogDebug("MangaDex Search URL: {Url}", url);

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.UserAgent.ParseAdd(_options.UserAgent);
                var response = await _httpClient.SendAsync(request, ct);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("MangaDex API returned: {StatusCode} - {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
                    return new List<MangaSourceEntity>();
                }

                var json = await response.Content.ReadAsStringAsync(ct);
                _logger.LogDebug("MangaDex Search Response: {Json}", json);

                var doc = JsonDocument.Parse(json);
                var searchResults = new List<MangaSourceEntity>();

                foreach (var manga in doc.RootElement.GetProperty("data").EnumerateArray())
                {
                    var mangaId = manga.GetProperty("id").GetString();
                    var mangaTitleObj = manga.GetProperty("attributes").GetProperty("title");
                    var mangaTitle = mangaTitleObj.TryGetProperty("en", out var en) ? en.GetString() : null;

                    string? description = null;
                    if (manga.GetProperty("attributes").TryGetProperty("description", out var descObj) && descObj.TryGetProperty("en", out var descEn))
                    {
                        description = descEn.GetString();
                    }

                    string? coverUrl = null;
                    if (manga.TryGetProperty("relationships", out var relationships))
                    {
                        foreach (var rel in relationships.EnumerateArray())
                        {
                            if (rel.GetProperty("type").GetString() == "cover_art")
                            {
                                var coverId = rel.GetProperty("id").GetString();
                                if (rel.TryGetProperty("attributes", out var coverAttrs) && coverAttrs.TryGetProperty("fileName", out var fileNameProp))
                                {
                                    var fileName = fileNameProp.GetString();
                                    if (!string.IsNullOrEmpty(mangaId) && !string.IsNullOrEmpty(fileName))
                                    {
                                        coverUrl = $"{_options.CoverBaseUrl}/{mangaId}/{fileName}";
                                    }
                                }
                                break;
                            }
                        }
                    }
                    searchResults.Add(new MangaSourceEntity(mangaId ?? string.Empty, mangaTitle ?? string.Empty, description, coverUrl));
                }

                return searchResults;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling MangaDex API");
                return new List<MangaSourceEntity>();
            }
        }

        #endregion
    }
}
