using EC.RestfulToken.Server.Api.Models;

namespace EC.RestfulToken.Server.Api.Services;

public interface ITestContentService
{
    List<TestContent> GetTestData(CancellationToken cancellationToken = default);
}

public class TestContentService(ILogger<TestContentService> logger) : ITestContentService
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    private readonly ILogger<TestContentService> _logger = logger;
    
    private readonly Random _random = new();

    public List<TestContent> GetTestData(CancellationToken cancellationToken = default)
    {
        var ret = new List<TestContent>();

        var count = _random.Next(10, 40);
        for (var i = 0; i < count; i++)
        {
            ret.Add(new TestContent
            {
                TestContentId = Guid.NewGuid(),
                CreatedBy = $"Test{i + 1}",
                CreatedDate = DateTime.Now,
                ModifiedBy = $"Test{i + 1}",
                ModifiedDate = DateTime.Now,
                Content = RandomString(_random.Next(5, 20))
            });
        }

        _logger.LogInformation("Random Test data generated: {0}", count);

        return ret;
    }

    protected virtual string RandomString(int length)
    {        
        var segment = Enumerable.Repeat(chars, length);
        var characters = segment.Select(c => c[_random.Next(c.Length)]).ToArray();
        return new string(characters);
    }
}
