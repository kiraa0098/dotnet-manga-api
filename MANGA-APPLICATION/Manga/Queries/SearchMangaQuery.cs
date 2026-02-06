using MediatR;
using MANGA_APPLICATION.Abstractions.External;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MANGA_APPLICATION.Manga.Queries
{
    public record SearchMangaQuery(string Query) : IRequest<IReadOnlyList<MetaDataItem>>;

    public class SearchMangaQueryHandler : IRequestHandler<SearchMangaQuery, IReadOnlyList<MetaDataItem>>
    {
        private readonly IMangaSource _mangaSource;
        public SearchMangaQueryHandler(IMangaSource mangaSource)
        {
            _mangaSource = mangaSource;
        }
        public async Task<IReadOnlyList<MetaDataItem>> Handle(SearchMangaQuery request, CancellationToken cancellationToken)
        {
            return await _mangaSource.SearchAsync(request.Query, cancellationToken);
        }
    }
}
