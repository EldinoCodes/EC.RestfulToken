using EC.RestfulToken.Server.Api.Models;
using EC.RestfulToken.Server.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EC.RestfulToken.Server.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TestContentController(ITestContentService testContentService) : ControllerBase
{
    private readonly ITestContentService _testContentService = testContentService;

    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(List<TestContent>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var data = _testContentService.GetTestData(cancellationToken);

        return new JsonResult(data);
    }
}
