using EC.RestfulToken.Server.Api.Data.Repositories;
using EC.RestfulToken.Server.Api.Models.Auth;
using EC.RestfulToken.Server.Api.Models.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EC.RestfulToken.Server.Api.Services;

public interface IAuthService
{
    Task<AuthToken?> AuthorizeUser(Guid? domainId, Guid? userId, string? secret, CancellationToken cancellationToken = default);
}

internal class AuthService(IConfiguration configuration, IUserRepository userRepository) : IAuthService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IUserRepository _userRepository = userRepository;

    public virtual async Task<AuthToken?> AuthorizeUser(Guid? domainId, Guid? userId, string? secret, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.UserGetBySecretAsync(domainId, userId, secret, cancellationToken);
        if (user is null) return default;

        return GenerateAccessToken(user);
    }

    protected virtual AuthToken? GenerateAccessToken(User? user)
    {
        if (user is null) return default;

        var jwtKey = _configuration.GetValue<string>("Jwt:Key");
        var jwtIssuer = _configuration.GetValue<string>("Jwt:Issuer");
        var jwtLifespan = _configuration.GetValue<double>("Jwt:Lifespan");

        if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer)) return default;

        var claims = new List<Claim>()
        {
            new (JwtRegisteredClaimNames.Iss, jwtIssuer),
            new (JwtRegisteredClaimNames.Aud, jwtIssuer),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Sid, $"{user.UserId}"),
            new (JwtRegisteredClaimNames.Name, $"{user.UserName}"),
            new ("domainId", $"{user.DomainId}"),
            new ("domain", $"{user.Domain}")
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var securityToken = new JwtSecurityToken(jwtIssuer, jwtIssuer, claims, DateTime.Now, DateTime.Now.AddSeconds(jwtLifespan), credentials);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return new AuthToken()
        {
            TokenType = "Bearer",
            AccessToken = accessToken,
            ExpiresIn = jwtLifespan
        };
    }
}