namespace EC.RestfulToken.Server.Api.Models.Tenants;

public class Tenant : BaseModel
{
    public Guid? TenantId { get; set; }
    public string? Name { get; set; }
}
