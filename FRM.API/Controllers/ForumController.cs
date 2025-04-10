using Forum.Domain.Dtos;
using Forum.Domain.UseCases.CreateTopic;
using Forum.Domain.UseCases.GetForums;
using Microsoft.AspNetCore.Mvc;
namespace FRM.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ForumController : ControllerBase
{
    public ForumController()
    {
    }

    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<string>))]
    public async Task<IActionResult> Get(
        [FromServices] IGetForumsUseCase useCase,
        CancellationToken cancellationToken)
    {

        var forums = await useCase.Execute(cancellationToken);
        return Ok(forums);

    }

    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] Dtos.TopicDto topicDto,
        [FromServices] ICreateTopicUseCase topicCreateUseCase,
        CancellationToken cancellationToken)
    {
        await topicCreateUseCase.Execute(topicDto.ForumId, topicDto.Title, topicDto.UserId, cancellationToken);
        return NoContent();
        
    }
}