using MediatR;
using MANGA_DOMAIN.Abstractions.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using MANGA_APPLICATION.Manga.DTOs.Response;
using MANGA_APPLICATION.Manga.DTOs.Request;
using System.Threading.Tasks;

namespace MANGA_APPLICATION.Manga.Queries
{
    public record SearchMangaQuery(SearchMangaRequest Request) : IRequest<PagedMangaSearchResponse>;

    public class SearchMangaQueryHandler : IRequestHandler<SearchMangaQuery, PagedMangaSearchResponse>
    {
        private readonly IMangaSource _mangaSource;
        public SearchMangaQueryHandler(IMangaSource mangaSource)
        {
            _mangaSource = mangaSource;
        }
        public async Task<PagedMangaSearchResponse> Handle(SearchMangaQuery query, CancellationToken cancellationToken)
        {
            var req = query.Request;
            var entities = await _mangaSource.SearchAsync(req.Title, req.PageNumber, req.Languages, cancellationToken);
            var paged = entities.ToList();

            var searchResults = paged.Select(entity => new MangaSearchResult
            {
                MangaId = entity.Id,
                MangaTitle = entity.Title,
                MangaDescription = entity.Description,
                MangaCoverImageUrl = entity.CoverUrl,
                MangaAuthor = entity.Author,
                MangaArtist = entity.Artist,
                MangaTags = entity.Tags,
                MangaLanguage = entity.Language,
                Ongoing = entity.Ongoing,
                ChapterTotal = entity.ChapterTotal
            }).ToList();

            return new PagedMangaSearchResponse
            {
                SearchedTitle = req.Title,
                PageNumber = req.PageNumber,
                Languages = req.Languages,
                MangaList = searchResults
            };
        }
    }
}
