namespace MANGA_APPLICATION.Manga.DTOs.Response;

public class PagedMangaSearchResponse
{
    public required string SearchedTitle { get; set; }
    public required int PageNumber { get; set; }
    public string Languages { get; set; } = "all";
    //! REMOVED: No longer needed
    //! public required int PageSize { get; set; }
    // TODO: Commented out for now since total count is not currently available from the source, 
    // TODO: but can be added back when implemented
    // public int? TotalCount { get; set; }
    public required IReadOnlyList<MangaSearchResult> MangaList { get; set; } = [];
}

public class MangaSearchResult
{
    public required string MangaId { get; set; } = string.Empty;
    public required string MangaTitle { get; set; } = string.Empty;
    public required string MangaDescription { get; set; } = string.Empty;
    public string? MangaCoverImageUrl { get; set; }
    public string? MangaAuthor { get; set; }
    public string? MangaArtist { get; set; }
    public IReadOnlyList<string>? MangaTags { get; set; }
    public string? MangaLanguage { get; set; }
    public bool? Ongoing { get; set; }
    public int? ChapterTotal { get; set; }
}
