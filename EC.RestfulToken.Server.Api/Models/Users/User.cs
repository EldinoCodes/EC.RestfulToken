using EC.RestfulToken.Server.Api.Models.Tenants;

namespace EC.RestfulToken.Server.Api.Models.Users;

public class User : BaseModel
{
    public Guid? TenantId { get; set; }
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public Tenant? Tenant { get; set; }
    public List<UserSecret>? UserSecrets { get; set; }
}
