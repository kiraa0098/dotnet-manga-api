namespace MANGA_INFRASTRUCTURE.External.MangaDex;

public sealed class MangaDexOptions
{
    // TODO: Consider making these configurable via appsettings.json
    // TODO: Changes to proper domains and contact info
    public string UserAgent { get; init; } = "MangaApi/1.0 (+https://yourdomain.com; contact@yourdomain.com)";
    public string BaseUrl { get; init; } = "https://api.mangadex.org";
    public string CoverBaseUrl { get; init; } = "https://uploads.mangadex.org/covers";

    public string BuildSearchUrl(string title, int offset, string? languages = null)
    {
        int pageSize = MANGA_DOMAIN.Enums.MangaPagingConstants.DefaultPageSize;
        var url = $"{BaseUrl}/manga?title={Uri.EscapeDataString(title)}&limit={pageSize}&offset={offset}&includes[]=cover_art";
        if (!string.IsNullOrWhiteSpace(languages) && languages.ToLowerInvariant() != "all")
        {
            foreach (var lang in languages.Split(','))
            {
                url += $"&availableTranslatedLanguage[]={lang.Trim()}";
            }
        }
        return url;
    }

    public string BuildGetByIdUrl(string id)
    {
        return $"{BaseUrl}/manga/{id}?includes[]=cover_art&includes[]=author&includes[]=artist&includes[]=tag";
    }
}
