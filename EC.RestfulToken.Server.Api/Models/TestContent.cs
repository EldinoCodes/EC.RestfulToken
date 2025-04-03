namespace EC.RestfulToken.Server.Api.Models;

public class TestContent : BaseModel
{
    public Guid? TestContentId { get; set; }
    public string? Content { get; set; }
}
