namespace MANGA_APPLICATION.Manga.DTOs.Request;

public class SearchMangaRequest
{
    public required string Title { get; set; }
    public int PageNumber { get; set; } = 1;
    public string Languages { get; set; } = "all";
    // TODO: Add tags
    // public IReadOnlyList<string> Tags { get; set; } = [];
}
