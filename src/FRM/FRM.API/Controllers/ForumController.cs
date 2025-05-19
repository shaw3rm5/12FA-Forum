using Domain.Models;
using Forum.Application.UseCases.CreateForum;
using Forum.Application.UseCases.CreateTopic;
using Forum.Application.UseCases.GetForums;
using Forum.Application.UseCases.GetTopics;
using Microsoft.AspNetCore.Mvc;
namespace FRM.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ForumController : ControllerBase
{


    [HttpPost]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    [ProducesResponseType(201)]
    
    public async Task<IActionResult> CreateForum(
        [FromServices] ICreateForumUseCase useCase,
        [FromBody] ForumCreateCommand createCommand,
        CancellationToken cancellationToken)
    {
        var newForum = await useCase.Execute(createCommand, cancellationToken);
        return CreatedAtRoute(nameof(GetForums), newForum);
    }
    
    
    [HttpGet(Name = "GetForums")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<string>))]
    [ProducesResponseType(410)]
    public async Task<IActionResult> GetForums(
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
    
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    [ProducesResponseType(410)]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetTopics(
        [FromServices] IGetTopicsUseCase useCase,
        [FromQuery] GetTopicsCommand topics, 
        CancellationToken cancellationToken)
    {
        var (result, totalCount) = await useCase.Execute(topics, cancellationToken);
        return Ok(new {result, totalCount});
    }
    
}