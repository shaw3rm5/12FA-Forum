using Forum.Application.Authorization;
using Forum.Application.Exceptions;
using Forum.Application.UseCases.CreateTopic;
using Forum.Application.UseCases.GetForums;
using FRM.API.Dtos;
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

    [HttpPost("topics")]
    [ProducesResponseType(403)]
    [ProducesResponseType(410)]
    [ProducesResponseType(200)]
    public async Task<IActionResult> CreateTopic(
        [FromServices] ICreateTopicUseCase useCase,
        [FromBody] TopicDto topic,
        CancellationToken cancellationToken)
    {

        try
        {
            await useCase.Execute(topic.ForumId, topic.Title, cancellationToken);
            return Created();
        }
        catch(Exception exception)
        {
            return exception switch
            {
                IntentionManagerException => Forbid(),
                ForumNotFindException => StatusCode(StatusCodes.Status410Gone),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }
        
    }
    
}