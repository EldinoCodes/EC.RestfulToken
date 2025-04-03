namespace EC.RestfulToken.Server.Api.Models.Domains;

public class Domain : BaseModel
{
    public Guid? DomainId { get; set; }
    public string? DomainName { get; set; }
}
