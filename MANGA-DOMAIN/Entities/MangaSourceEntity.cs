namespace MANGA_DOMAIN.Entities;

public class MangaSourceEntity
{
    public string Id { get; }
    public string Title { get; }
    public string? Description { get; }
    public string? CoverUrl { get; }
    public string? Author { get; }
    public string? Artist { get; }
    public IReadOnlyList<string>? Tags { get; }
    public string? Language { get; }
    public bool? Ongoing { get; }
    public int? ChapterTotal { get; }

    public MangaSourceEntity(
        string id,
        string title,
        string? description,
        string? coverUrl,
        string? author,
        string? artist,
        IReadOnlyList<string>? tags,
        string? language,
        bool? ongoing,
        int? chapterTotal)
    {
        Id = id;
        Title = title;
        Description = description;
        CoverUrl = coverUrl;
        Author = author;
        Artist = artist;
        Tags = tags;
        Language = language;
        Ongoing = ongoing;
        ChapterTotal = chapterTotal;
    }
}