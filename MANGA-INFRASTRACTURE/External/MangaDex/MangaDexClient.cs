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
                var url = _options.BuildGetByIdUrl(id);
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
                string? mangaDescription = manga.GetProperty("attributes").TryGetProperty("description", out var descObj) && descObj.TryGetProperty("en", out var descEn) ? descEn.GetString() : null;
                string? mangaCoverUrl = null;
                string? mangaAuthor = null;
                string? mangaArtist = null;
                List<string>? mangaTags = null;

                if (manga.TryGetProperty("relationships", out var relationships))
                {
                    mangaTags = new List<string>();
                    foreach (var rel in relationships.EnumerateArray())
                    {
                        var relType = rel.GetProperty("type").GetString();
                        switch (relType)
                        {
                            case "cover_art":
                                if (rel.TryGetProperty("attributes", out var coverAttrs) && coverAttrs.TryGetProperty("fileName", out var fileNameProp))
                                {
                                    var fileName = fileNameProp.GetString();
                                    if (!string.IsNullOrEmpty(mangaId) && !string.IsNullOrEmpty(fileName))
                                    {
                                        mangaCoverUrl = $"{_options.CoverBaseUrl}/{mangaId}/{fileName}";
                                    }
                                }
                                break;
                            case "author":
                                if (rel.TryGetProperty("attributes", out var authorAttrs) && authorAttrs.TryGetProperty("name", out var authorNameProp))
                                {
                                    mangaAuthor = authorNameProp.GetString();
                                }
                                break;
                            case "artist":
                                if (rel.TryGetProperty("attributes", out var artistAttrs) && artistAttrs.TryGetProperty("name", out var artistNameProp))
                                {
                                    mangaArtist = artistNameProp.GetString();
                                }
                                break;
                            case "tag":
                                if (rel.TryGetProperty("attributes", out var tagAttrs) && tagAttrs.TryGetProperty("name", out var tagNameObj))
                                {
                                    if (tagNameObj.TryGetProperty("en", out var tagEn))
                                    {
                                        mangaTags.Add(tagEn.GetString()!);
                                    }
                                }
                                break;
                        }
                    }
                }

                var mangaDetails = new List<MangaSourceEntity>();
                mangaDetails.Add(new MangaSourceEntity(
                    mangaId ?? string.Empty,
                    mangaTitle ?? string.Empty,
                    mangaDescription,
                    mangaCoverUrl,
                    mangaAuthor,
                    mangaArtist,
                    mangaTags,
                    null, // TODO: Null for now, need to determine how to handle language for single manga retrieval
                    null, // TODO: Null for now, need to determine how to handle ongoing status for single manga retrieval
                    null // TODO: Null for now, need to determine how to handle chapter total for single manga retrieval
                    ));

                return mangaDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling MangaDex API for details");
                return new List<MangaSourceEntity>();
            }
        }
        #endregion


        #region SearchAsync
        public async Task<IReadOnlyList<MangaSourceEntity>> SearchAsync(string title, int pageNumber, string languages, CancellationToken ct)
        {
            try
            {
                int offset = MANGA_DOMAIN.Enums.MangaPagingConstants.GetOffset(pageNumber);
                var url = _options.BuildSearchUrl(title, offset, languages);
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
                    try
                    {
                        var mangaId = manga.GetProperty("id").GetString();
                        var attr = manga.GetProperty("attributes");
                        var mangaTitleObj = attr.GetProperty("title");
                        var mangaTitle = mangaTitleObj.TryGetProperty("en", out var en) ? en.GetString() : null;

                        string? mangaDescription = null;
                        if (attr.TryGetProperty("description", out var descObj) && descObj.TryGetProperty("en", out var descEn))
                        {
                            mangaDescription = string.IsNullOrWhiteSpace(descEn.GetString()) ? "no description" : descEn.GetString();
                        }
                        if (string.IsNullOrWhiteSpace(mangaDescription))
                            mangaDescription = "no description";

                        string? mangaLanguage = attr.TryGetProperty("originalLanguage", out var langProp) ? langProp.GetString() : null;

                        bool? ongoing = null;
                        if (attr.TryGetProperty("status", out var statusProp))
                        {
                            var status = statusProp.GetString();
                            if (!string.IsNullOrEmpty(status))
                                ongoing = status.ToLowerInvariant() == "ongoing";
                        }

                        int? chapterTotal = null;
                        if (attr.TryGetProperty("lastChapter", out var lastChapterProp))
                        {
                            if (int.TryParse(lastChapterProp.GetString(), out var chTotal))
                                chapterTotal = chTotal;
                        }

                        string? mangaCoverUrl = null;
                        List<string>? mangaTags = null;
                        string? mangaAuthor = null;
                        string? mangaArtist = null;
                        if (manga.TryGetProperty("relationships", out var relationships))
                        {
                            mangaTags = new List<string>();
                            foreach (var rel in relationships.EnumerateArray())
                            {
                                var relType = rel.GetProperty("type").GetString();
                                if (relType == "cover_art")
                                {
                                    if (rel.TryGetProperty("attributes", out var coverAttrs) && coverAttrs.TryGetProperty("fileName", out var fileNameProp))
                                    {
                                        var fileName = fileNameProp.GetString();
                                        if (!string.IsNullOrEmpty(mangaId) && !string.IsNullOrEmpty(fileName))
                                        {
                                            mangaCoverUrl = $"{_options.CoverBaseUrl}/{mangaId}/{fileName}";
                                        }
                                    }
                                }
                                else if (relType == "tag")
                                {
                                    if (rel.TryGetProperty("attributes", out var tagAttrs) && tagAttrs.TryGetProperty("name", out var tagNameObj))
                                    {
                                        if (tagNameObj.TryGetProperty("en", out var tagEn))
                                        {
                                            mangaTags.Add(tagEn.GetString()!);
                                        }
                                    }
                                }
                                else if (relType == "author")
                                {
                                    if (rel.TryGetProperty("attributes", out var authorAttrs) && authorAttrs.TryGetProperty("name", out var authorNameProp))
                                    {
                                        mangaAuthor = authorNameProp.GetString();
                                    }
                                }
                                else if (relType == "artist")
                                {
                                    if (rel.TryGetProperty("attributes", out var artistAttrs) && artistAttrs.TryGetProperty("name", out var artistNameProp))
                                    {
                                        mangaArtist = artistNameProp.GetString();
                                    }
                                }
                            }
                        }
                        searchResults.Add(new MangaSourceEntity(
                            mangaId ?? string.Empty,
                            mangaTitle ?? string.Empty,
                            mangaDescription,
                            mangaCoverUrl,
                            mangaAuthor,
                            mangaArtist,
                            mangaTags,
                            mangaLanguage,
                            ongoing,
                            chapterTotal));
                    }
                    catch (Exception itemEx)
                    {
                        _logger.LogWarning(itemEx, "Failed to parse manga item in search results. Skipping item.");
                    }
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
