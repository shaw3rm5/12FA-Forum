using Forum.Application.Authentication;
using Forum.Application.UseCases.SignIn;
using Forum.Application.UseCases.SignUp;
using FRM.API.Authentication;
using FRM.API.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FRM.API.Controllers;

[ApiController]
[Route("account")]
public class AccountController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SignUp(
        [FromBody] SignUpDto request,
        [FromServices] ISignUpUseCase useCase,
        CancellationToken cancellationToken)
    {
        var identity = await useCase.Execute(new SignUpCommand(request.Login, request.Password), cancellationToken);
        return Ok(identity);
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(
        [FromBody] SignInDto request,
        [FromServices] ISignInUseCase useCase,
        [FromServices] IAuthTokenStorage storage,
        CancellationToken cancellationToken
    )
    {
        var (identity, authToken) = await useCase.Execute(new SignInCommand(request.Login, request.Password), cancellationToken); 
        storage.Store(HttpContext, authToken);
        return Ok(identity);
    }
}