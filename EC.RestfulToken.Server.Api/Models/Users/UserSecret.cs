namespace EC.RestfulToken.Server.Api.Models.Users;

public class UserSecret : BaseModel
{
    public Guid? DomainId { get; set; }
    public Guid? UserId { get; set; }
    public string? Secret { get; set; }
}
