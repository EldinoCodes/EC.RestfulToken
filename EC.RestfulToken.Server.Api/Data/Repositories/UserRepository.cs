using Dapper;
using EC.RestfulToken.Server.Api.Data.Contexts;
using EC.RestfulToken.Server.Api.Models.Tenants;
using EC.RestfulToken.Server.Api.Models.Users;

namespace EC.RestfulToken.Server.Api.Data.Repositories;

public interface IUserRepository
{
    Task<User?> UserGetBySecretAsync(Guid? tenantId, Guid? clientId, string? clientSecret, CancellationToken cancellationToken = default);
}

internal class UserRepository(IDbReadonlyContext dbReadonlyContext) : IUserRepository
{
    private readonly IDbReadonlyContext _dbReadonlyContext = dbReadonlyContext;

    public async Task<User?> UserGetBySecretAsync(Guid? tenantId, Guid? clientId, string? clientSecret, CancellationToken cancellationToken = default)
    {
        /*
         * honestly this validation isnt really needed as 
         * in theory the sql shouldnt return anything
         */
        if (tenantId is null || tenantId == Guid.Empty) return default;
        if (clientId is null || clientId == Guid.Empty) return default;
        if (string.IsNullOrEmpty(clientSecret)) return default;

        const string sSql = @"
            SELECT 
                u.[TenantId], u.[UserId], u.[CreatedDate], u.[CreatedBy], u.[ModifiedDate], u.[ModifiedBy], u.[UserName], u.[Email], 
                t.[TenantId], t.[Name]
            FROM [dbo].[User] u 
            JOIN [dbo].[Tenant] t ON t.TenantId = u.TenantId
            JOIN [dbo].[UserSecret] us ON us.TenantId = u.TenantId AND us.UserId = u.UserId
            WHERE u.UserId = @UserId 
            AND u.TenantId = @TenantId
            AND us.Secret = @Secret;
        ";

        var param = new { TenantId = tenantId, UserId = clientId, Secret = clientSecret };

        using var connection = _dbReadonlyContext.Connection;

        var sCmd = new CommandDefinition(sSql, param, cancellationToken: cancellationToken);

        static User objectMap(User user, Tenant tenant)
        {
            user.Tenant = tenant;
            return user;
        }

        var results = await connection.QueryAsync(sCmd, (Func<User, Tenant, User>)objectMap, splitOn: "TenantId, TenantId");

        return results.FirstOrDefault();
    }
}