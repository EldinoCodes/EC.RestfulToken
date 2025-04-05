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
    private readonly string _jwtKey = configuration.GetValue<string?>("Jwt:Key") ?? throw new ArgumentNullException("setting 'Jwt:Key' is missing");
    private readonly string _jwtIssuer = configuration.GetValue<string?>("Jwt:Issuer") ?? throw new ArgumentNullException("setting 'Jwt:Issuer' is missing");
    private readonly double _jwtLifespan = configuration.GetValue<double?>("Jwt:Lifespan") ?? throw new ArgumentNullException("setting 'Jwt:Lifespan' is missing");

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
        if (user?.Tenant is null) return default;

        var claims = new List<Claim>()
        {
            new (JwtRegisteredClaimNames.Iss, _jwtIssuer),
            new (JwtRegisteredClaimNames.Aud, _jwtIssuer),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Sid, $"{user.UserId}"),
            new (JwtRegisteredClaimNames.Name, $"{user.UserName}"),

            // ** claim types are used for different things **
            new (ClaimTypes.Name, $"{user.UserName}"),
            new ("domainId", $"{user.Tenant.TenantId}"),
            new ("domain", $"{user.Tenant.Name}")
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var securityToken = new JwtSecurityToken(_jwtIssuer, _jwtIssuer, claims, DateTime.Now, DateTime.Now.AddSeconds(_jwtLifespan), credentials);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return new AuthToken()
        {
            TokenType = "Bearer",
            AccessToken = accessToken,
            ExpiresIn = _jwtLifespan
        };
    }
}