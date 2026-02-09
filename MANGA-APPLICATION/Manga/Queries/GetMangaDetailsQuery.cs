using MediatR;
using MANGA_DOMAIN.Abstractions.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MANGA_DOMAIN.Entities;


namespace MANGA_APPLICATION.Manga.Queries
{
    public record GetMangaDetailsQuery(string Id) : IRequest<IReadOnlyList<MangaSourceEntity>>;

    public class GetMangaDetailsQueryHandler : IRequestHandler<GetMangaDetailsQuery, IReadOnlyList<MangaSourceEntity>>
    {
        private readonly IMangaSource _mangaSource;
        public GetMangaDetailsQueryHandler(IMangaSource mangaSource)
        {
            _mangaSource = mangaSource;
        }
        public async Task<IReadOnlyList<MangaSourceEntity>> Handle(GetMangaDetailsQuery request, CancellationToken cancellationToken)
        {
            // TODO: Fetch and include chapter totals in the result when available
            return await _mangaSource.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
