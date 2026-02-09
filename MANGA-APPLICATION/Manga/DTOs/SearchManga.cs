namespace MANGA_APPLICATION.Manga.DTOs;

public class PagedMangaSearchResult
{
    public required string SearchedTitle { get; set; }
    public required int PageNumber { get; set; }
    // REMOVE: No longer needed
    // public required int PageSize { get; set; }
    // TODO: Commented out for now since total count is not currently available from the source, 
    // TODO: but can be added back when implemented
    // public int? TotalCount { get; set; }
    public required IReadOnlyList<MangaSearchResult> Items { get; set; } = [];
}

public class MangaSearchResult
{
    public required string MangaId { get; set; } = string.Empty;
    public required string MangaTitle { get; set; } = string.Empty;
    public required string MangaDescription { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    // TODO: Add property for chapter total when available
    // public int ChapterTotal { get; set; }
}
