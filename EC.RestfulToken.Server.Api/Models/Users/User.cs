using EC.RestfulToken.Server.Api.Models.Domains;

namespace EC.RestfulToken.Server.Api.Models.Users;

public class User : BaseModel
{
    public Guid? DomainId { get; set; }
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Domain { get; set; }
    public List<UserSecret>? UserSecrets { get; set; }
}
