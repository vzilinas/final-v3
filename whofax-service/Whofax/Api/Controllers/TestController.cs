using MediatR;
using Microsoft.AspNetCore.Mvc;
using Whofax.Application.Commands.Test;

namespace Whofax.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IMediator _mediator;

    public TestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] string value)
    {
        await _mediator.Send(new TestCommand(value));

        return Ok();
    }
}
