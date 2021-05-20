using Aimrank.Agones.Api.Contracts;
using Aimrank.Agones.Infrastructure.CSGO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Aimrank.Agones.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CSGOController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IServerEventMapper _serverEventMapper;

        public CSGOController(IMediator mediator, IServerEventMapper serverEventMapper)
        {
            _mediator = mediator;
            _serverEventMapper = serverEventMapper;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessServerEvent(ProcessServerEventRequest request)
        {
            var command = _serverEventMapper.Map(request.Name, request.Data);
            if (command is null)
            {
                return BadRequest();
            }

            await _mediator.Send(command);

            return Accepted();
        }
    }
}