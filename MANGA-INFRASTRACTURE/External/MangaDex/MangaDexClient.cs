using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MANGA_APPLICATION.Abstractions.External;
using MANGA_APPLICATION.Manga.DTOs;
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

        public async Task<MangaSourceItem?> GetByIdAsync(string id, CancellationToken ct)
        {
            try
            {
                var url = $"{_options.BaseUrl}/manga/{id}";
                var response = await _httpClient.GetAsync(url, ct);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("MangaDex API returned: {StatusCode} - {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
                    return null;
                }
                var json = await response.Content.ReadAsStringAsync(ct);
                var doc = JsonDocument.Parse(json);
                var manga = doc.RootElement.GetProperty("data");
                var mangaId = manga.GetProperty("id").GetString();
                var titleObj = manga.GetProperty("attributes").GetProperty("title");
                var title = titleObj.TryGetProperty("en", out var en) ? en.GetString() : null;
                var description = manga.GetProperty("attributes").TryGetProperty("description", out var descObj) && descObj.TryGetProperty("en", out var descEn) ? descEn.GetString() : null;
                // CoverUrl and chapters can be added later
                return new MangaSourceItem(mangaId ?? string.Empty, title ?? string.Empty, description, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling MangaDex API for details");
                return null;
            }
        }

        public async Task<IReadOnlyList<MangaSourceItem>> SearchAsync(string query, CancellationToken ct)
        {
            try
            {
                var url = $"{_options.BaseUrl}/manga?title={Uri.EscapeDataString(query)}&limit=10";
                var response = await _httpClient.GetAsync(url, ct);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("MangaDex API returned: {StatusCode} - {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
                    return new List<MangaSourceItem>();
                }

                var json = await response.Content.ReadAsStringAsync(ct);
                var doc = JsonDocument.Parse(json);
                var results = new List<MangaSourceItem>();
                foreach (var manga in doc.RootElement.GetProperty("data").EnumerateArray())
                {
                    var id = manga.GetProperty("id").GetString();
                    var titleObj = manga.GetProperty("attributes").GetProperty("title");
                    var title = titleObj.TryGetProperty("en", out var en) ? en.GetString() : null;
                    results.Add(new MangaSourceItem(id ?? string.Empty, title ?? string.Empty, null, null));
                }
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling MangaDex API");
                return new List<MangaSourceItem>();
            }
        }
    }
}
