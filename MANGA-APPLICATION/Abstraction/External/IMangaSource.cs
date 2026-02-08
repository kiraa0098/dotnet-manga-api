using MANGA_APPLICATION.Manga.DTOs;
namespace MANGA_APPLICATION.Abstractions.External;

public interface IMangaSource
{
    Task<MangaSourceItem?> GetByIdAsync(string id, CancellationToken ct);
    Task<IReadOnlyList<MangaSourceItem>> SearchAsync(string query, CancellationToken ct);
}

