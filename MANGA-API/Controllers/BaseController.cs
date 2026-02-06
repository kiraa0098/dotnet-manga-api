using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MANGA_API.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        // Leave empty for now
        // This can be used for common functionalities across all controllers in the future

        private IMediator? _mediator;
        protected IMediator? Mediator => _mediator ??= (_mediator = HttpContext.RequestServices.GetService<IMediator>());
    }
}