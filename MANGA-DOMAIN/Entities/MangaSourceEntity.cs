namespace MANGA_DOMAIN.Entities;

public class MangaSourceEntity
{
    public string Id { get; }
    public string Title { get; }
    public string? Description { get; }
    public string? CoverUrl { get; }

    public MangaSourceEntity(string id, string title, string? description, string? coverUrl)
    {
        Id = id;
        Title = title;
        Description = description;
        CoverUrl = coverUrl;
    }
}