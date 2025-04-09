using Forum.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [FromServices] ForumDbContext dbContext,
        CancellationToken cancellationToken)
    {

        var titles = await dbContext.Forums.Select(f => f.Title).ToArrayAsync(cancellationToken);
        
        return Ok(titles);

    }
    
}