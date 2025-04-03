using EC.RestfulToken.Server.Api.Models.Auth;
using EC.RestfulToken.Server.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace EC.RestfulToken.Server.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TokenController(IConfiguration configuration, IAuthService authService) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IAuthService _authService = authService;

    //[Authorize]
    [AllowAnonymous]
    [HttpPost]
    [Consumes(MediaTypeNames.Application.FormUrlEncoded)]
    [ProducesResponseType(typeof(AuthToken), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> OnPost([FromForm] AuthRequest? authRequest, CancellationToken cancellationToken = default)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authorization)) return Unauthorized();
        if (!AuthenticationHeaderValue.TryParse(authorization, out var header)) return Unauthorized();

        if (header is null) return Unauthorized();
        if (!header.Scheme.Equals("bearer", StringComparison.InvariantCultureIgnoreCase)) return Unauthorized();

        if (authRequest == null) return Unauthorized();

        var token = await _authService.AuthorizeUser(authRequest.DomainId, authRequest.UserId, header.Parameter, cancellationToken);
        if (token is null) return Unauthorized();

        return new JsonResult(token);
    }
}