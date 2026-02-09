using MediatR;
using MANGA_DOMAIN.Abstractions.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using MANGA_APPLICATION.Manga.DTOs;
using System.Threading.Tasks;

namespace MANGA_APPLICATION.Manga.Queries
{
    public record SearchMangaQuery(string Title, int PageNumber) : IRequest<PagedMangaSearchResult>;

    public class SearchMangaQueryHandler : IRequestHandler<SearchMangaQuery, PagedMangaSearchResult>
    {
        private readonly IMangaSource _mangaSource;
        public SearchMangaQueryHandler(IMangaSource mangaSource)
        {
            _mangaSource = mangaSource;
        }
        public async Task<PagedMangaSearchResult> Handle(SearchMangaQuery request, CancellationToken cancellationToken)
        {
            var entities = await _mangaSource.SearchAsync(request.Title, request.PageNumber, cancellationToken);
            var paged = entities
                .ToList();

            var searchResults = paged.Select(entity => new MangaSearchResult
            {
                MangaId = entity.Id,
                MangaTitle = entity.Title,
                MangaDescription = entity.Description,
                CoverImageUrl = entity.CoverUrl
                // TODO: Map chapter total when available
            }).ToList();

            return new PagedMangaSearchResult
            {
                SearchedTitle = request.Title,
                PageNumber = request.PageNumber,
                // TODO: Commented out for now since total count is not currently available from the source, 
                // TODO: but can be added back when implemented
                // TotalCount = entities.Count,
                Items = searchResults
            };
        }
    }
}
