using Microsoft.AspNetCore.Mvc;
using MediatR;
using MANGA_APPLICATION.Manga.Queries.GetMangaDetailsQuery;
using System.ComponentModel;

namespace MANGA_API.Controllers
{
    [Route("manga-api/manga/details")]
    public class MangaDetailsController : BaseController
    {
        [HttpGet("{id}")]
        [Description("Get Manga Details")]
        public async Task<IActionResult> GetDetails([FromQuery] string query)
        {
            var result = await Mediator!.Send(new GetMangaDetailsQuery(query));
            return Ok(result);
        }
    }
}