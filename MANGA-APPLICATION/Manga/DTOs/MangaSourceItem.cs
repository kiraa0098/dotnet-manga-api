namespace MANGA_APPLICATION.Manga.DTOs;

public record MangaSourceItem(
    string Id,
    string Title,
    string? Description,
    string? CoverUrl
    // In the future: IReadOnlyList<ChapterSourceItem>? Chapters
);
