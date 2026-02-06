using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MANGA_APPLICATION.Abstractions.External;
using MANGA_INFRASTRUCTURE.External.MangaDex;

namespace manga_infrastracture.External
{
    public class MangaDexClient : IMangaSource
    {
        private readonly HttpClient _httpClient;
        private readonly MangaDexOptions _options;
        public MangaDexClient(HttpClient httpClient, MangaDexOptions options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        public async Task<MetaDataItem?> GetByIdAsync(string id, CancellationToken ct)
        {
            // Not yet implemented
            return null;
        }

        public async Task<IReadOnlyList<MetaDataItem>> SearchAsync(string query, CancellationToken ct)
        {
            var url = $"{_options.BaseUrl}/manga?title={Uri.EscapeDataString(query)}&limit=10";
            var response = await _httpClient.GetAsync(url, ct);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync(ct);
            var doc = JsonDocument.Parse(json);
            var results = new List<MetaDataItem>();
            foreach (var manga in doc.RootElement.GetProperty("data").EnumerateArray())
            {
                var id = manga.GetProperty("id").GetString();
                var titleObj = manga.GetProperty("attributes").GetProperty("title");
                var title = titleObj.TryGetProperty("en", out var en) ? en.GetString() : null;
                results.Add(new MetaDataItem(id ?? string.Empty, title ?? string.Empty, null, null));
            }
            return results;
        }
    }
}
