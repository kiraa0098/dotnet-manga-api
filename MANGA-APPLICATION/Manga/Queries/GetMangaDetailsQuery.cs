using MediatR;
using MANGA_DOMAIN.Abstractions.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MANGA_DOMAIN.Entities;
using MANGA_APPLICATION.Manga.DTOs.Response;
using MANGA_APPLICATION.Manga.DTOs.Request;


namespace MANGA_APPLICATION.Manga.Queries
{
    public record GetMangaDetailsQuery(string Id) : IRequest<IReadOnlyList<MangaDetailsResponse>>;

    public class GetMangaDetailsQueryHandler : IRequestHandler<GetMangaDetailsQuery, IReadOnlyList<MangaDetailsResponse>>
    {
        private readonly IMangaSource _mangaSource;
        public GetMangaDetailsQueryHandler(IMangaSource mangaSource)
        {
            _mangaSource = mangaSource;
        }
        public async Task<IReadOnlyList<MangaDetailsResponse>> Handle(GetMangaDetailsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _mangaSource.GetByIdAsync(request.Id, cancellationToken);
            var results = new List<MangaDetailsResponse>();
            foreach (var entity in entities)
            {
                results.Add(new MangaDetailsResponse
                {
                    MangaId = entity.Id,
                    MangaTitle = entity.Title,
                    MangaDescription = entity.Description,
                    CoverImageUrl = entity.CoverUrl,
                    Author = entity.Author,
                    Artist = entity.Artist,
                    Tags = entity.Tags
                });
            }
            return results;
        }
    }
}
