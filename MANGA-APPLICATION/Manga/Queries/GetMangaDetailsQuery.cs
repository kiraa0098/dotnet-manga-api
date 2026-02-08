using MediatR;
using MANGA_APPLICATION.Abstractions.External;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace MANGA_APPLICATION.Manga.Queries
{
    public record GetMangaDetailsQuery(string Query) : IRequest<IReadOnlyList<MangaSourceItem>>;

    public class GetMangaDetailsQueryHandler : IRequestHandler<GetMangaDetailsQuery, IReadOnlyList<MangaSourceItem>>
    {
        private readonly IMangaSource _mangaSource;
        public GetMangaDetailsQueryHandler(IMangaSource mangaSource)
        {
            _mangaSource = mangaSource;
        }
        public async Task<IReadOnlyList<MangaSourceItem>> Handle(GetMangaDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _mangaSource.GetByIdAsync(request.Query, cancellationToken);
        }
    }
}
