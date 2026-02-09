using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MANGA_DOMAIN.Entities;

namespace MANGA_DOMAIN.Abstractions.Interfaces;

public interface IMangaSource
{
    Task<IReadOnlyList<MangaSourceEntity>> GetByIdAsync(string id, CancellationToken ct);
    Task<IReadOnlyList<MangaSourceEntity>> SearchAsync(string title, int pageNumber, CancellationToken ct);
}