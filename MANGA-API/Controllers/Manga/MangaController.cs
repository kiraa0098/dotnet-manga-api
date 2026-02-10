using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MANGA_APPLICATION.Manga.Queries;
using MANGA_APPLICATION.Manga.DTOs.Request;

namespace MANGA_API.Controllers
{
    [Route("manga-api/manga")]
    public class MangaController : BaseController
    {
        [HttpGet("search")]
        [Description("Search Manga")]
        public async Task<IActionResult> Search([FromQuery] SearchMangaRequest requestDto)
        {
            var result = await Mediator!.Send(new SearchMangaQuery(requestDto));
            return Ok(result);
        }
    }

    [Route("manga-api/manga/details")]
    public class MangaDetailsController : BaseController
    {
        [HttpGet("{id}")]
        [Description("Get Manga Details")]
        // TODO: Refactor GetDetails to accept a request DTO via model binding, like Search. (See Search endpoint for reference.)
        public async Task<IActionResult> GetDetails([FromRoute] string id)
        {
            var result = await Mediator!.Send(new GetMangaDetailsQuery(id));
            return Ok(result);
        }
    }
}