using EC.RestfulToken.Server.Api.Models.Auth;
using EC.RestfulToken.Server.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace EC.RestfulToken.Server.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TokenController(ILogger<TokenController> logger, IAuthService authService) : ControllerBase
{
    private readonly ILogger<TokenController> _logger = logger;
    private readonly IAuthService _authService = authService;

    [AllowAnonymous]
    [HttpPost("{tenantId:guid}")]
    [Consumes(MediaTypeNames.Application.FormUrlEncoded)]
    [ProducesResponseType(typeof(AuthToken), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> OnPostAsync(Guid? tenantId, [FromForm] AuthRequest? authRequest, CancellationToken cancellationToken = default)
    {
        if (tenantId is null || tenantId == Guid.Empty) return Unauthorized();
        if (authRequest?.ClientId is null || authRequest?.ClientId == Guid.Empty) return Unauthorized();
        if (string.IsNullOrEmpty(authRequest?.ClientSecret)) return Unauthorized();

        var token = await _authService.AuthorizeUser(tenantId, authRequest.ClientId, authRequest.ClientSecret, cancellationToken);
        if (token is null) return Unauthorized();

        _logger.LogWarning($"token generated for user: '{authRequest?.ClientId}'");

        return new JsonResult(token);
    }
}