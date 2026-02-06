namespace MANGA_APPLICATION.Abstractions.External;

public interface IMangaSource
{
    Task<MetaDataItem?> GetByIdAsync(string id, CancellationToken ct);
    Task<IReadOnlyList<MetaDataItem>> SearchAsync(string query, CancellationToken ct);
}

// Add more record items as needed

public record MetaDataItem(
    string Id,
    string Title,
    string? Description,
    string? CoverUrl
);
