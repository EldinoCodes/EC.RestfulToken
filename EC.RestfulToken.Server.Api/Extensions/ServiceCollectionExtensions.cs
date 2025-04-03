using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EC.RestfulToken.Server.Api;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRestfulTokenApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtKey = configuration.GetValue<string>("Jwt:Key");
        ArgumentException.ThrowIfNullOrEmpty(nameof(jwtKey));
        var jwtIssuer = configuration.GetValue<string>("Jwt:Issuer");
        ArgumentException.ThrowIfNullOrEmpty(nameof(jwtIssuer));

        services.AddOptions<MvcOptions>().Configure(o =>
        {
            var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
            o.Filters.Add(new AuthorizeFilter(policy));
        });

        var authBuilder = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
        authBuilder
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? "")),
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }
}

