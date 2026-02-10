namespace MANGA_APPLICATION.Manga.DTOs.Response;

public class MangaDetailsResponse
{
    public required string MangaId { get; set; }
    public required string MangaTitle { get; set; }
    public string? MangaDescription { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Author { get; set; }
    public string? Artist { get; set; }
    public IReadOnlyList<string>? Tags { get; set; }
}
