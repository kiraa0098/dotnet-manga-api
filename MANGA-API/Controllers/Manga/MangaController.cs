using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MANGA_APPLICATION.Manga.Queries;

namespace MANGA_API.Controllers
{
    [Route("manga-api/manga")]
    public class MangaController : BaseController
    {
        [HttpGet("search")]
        [Description("Search Manga")]
        public async Task<IActionResult> Search([FromQuery] string title, [FromQuery] int pageNumber = 1)
        {
            var result = await Mediator!.Send(new SearchMangaQuery(title, pageNumber));
            return Ok(result);
        }
    }

    [Route("manga-api/manga/details")]
    public class MangaDetailsController : BaseController
    {
        [HttpGet("{id}")]
        [Description("Get Manga Details")]
        public async Task<IActionResult> GetDetails([FromRoute] string id)
        {
            var result = await Mediator!.Send(new GetMangaDetailsQuery(id));
            return Ok(result);
        }
    }
}