using Forum.Application.UseCases.CreateTopic;
using Forum.Application.UseCases.GetForums;
using Forum.Application.UseCases.GetTopics;
using Microsoft.AspNetCore.Mvc;
namespace FRM.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ForumController : ControllerBase
{

    
    [HttpGet(Name = "GetForums")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<string>))]
    public async Task<IActionResult> Get(
        [FromServices] IGetForumsUseCase useCase,
        CancellationToken cancellationToken)
    {

        var forums = await useCase.Execute(cancellationToken);
        return Ok(forums);

    }

    [HttpPost("topic")]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    [ProducesResponseType(410)]
    [ProducesResponseType(200)]
    public async Task<IActionResult> CreateTopic(
        [FromServices] ICreateTopicUseCase useCase,
        [FromBody] CreateTopicCommand topic,
        CancellationToken cancellationToken)
    {
        await useCase.Execute(topic, cancellationToken);
        return Created();
    }


    [HttpGet("topics")]
    public async Task<IActionResult> GetTopics(
        [FromServices] IGetTopicsUseCase useCase,
        [FromQuery] GetTopicsCommand topics, 
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(topics, cancellationToken);
        return Ok(result.resources);
    }
    
}