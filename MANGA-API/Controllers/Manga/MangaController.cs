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
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var result = await Mediator!.Send(new SearchMangaQuery(query));
            return Ok(result);
        }
    }
}