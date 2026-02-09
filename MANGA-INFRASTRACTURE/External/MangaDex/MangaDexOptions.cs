namespace MANGA_INFRASTRUCTURE.External.MangaDex;

public sealed class MangaDexOptions
{
    public string UserAgent { get; init; } = "MangaApi/1.0 (+https://yourdomain.com; contact@yourdomain.com)";
    public string BaseUrl { get; init; } = "https://api.mangadex.org";
    public string CoverBaseUrl { get; init; } = "https://uploads.mangadex.org/covers";

    public string BuildSearchUrl(string title, int offset)
    {
        int pageSize = MANGA_DOMAIN.Enums.MangaPagingConstants.DefaultPageSize;
        return $"{BaseUrl}/manga?title={Uri.EscapeDataString(title)}&limit={pageSize}&offset={offset}&includes[]=cover_art";
    }
}
