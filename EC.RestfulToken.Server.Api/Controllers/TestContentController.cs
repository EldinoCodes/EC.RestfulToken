using EC.RestfulToken.Server.Api.Models;
using EC.RestfulToken.Server.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EC.RestfulToken.Server.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TestContentController(ILogger<TestContentController> logger, ITestContentService testContentService) : ControllerBase
{
    private readonly ILogger<TestContentController> _logger = logger;
    private readonly ITestContentService _testContentService = testContentService;

    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(List<TestContent>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult OnGet(CancellationToken cancellationToken = default)
    {
        var data = _testContentService.GetTestData(cancellationToken);

        /*
         * the requests here are authed, so we should have user information...
         */

        _logger.LogWarning($"TestContent returned {data?.Count} items to {User.Identity?.Name} who has {User.Claims.Count()} claims");

        return new JsonResult(data);
    }
}
