namespace EC.RestfulToken.Server.Api.Models.Auth;

/*
 * no need to inherit from BaseModel, those props dont have value here
 * this just covers the input to authorize a user
 */
public class AuthRequest
{
    public virtual Guid? ClientId { get; set; }
    public virtual string? ClientSecret { get; set; }
}
