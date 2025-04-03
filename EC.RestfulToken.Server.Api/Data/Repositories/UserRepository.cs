using Dapper;
using EC.RestfulToken.Server.Api.Data.Contexts;
using EC.RestfulToken.Server.Api.Models.Users;

namespace EC.RestfulToken.Server.Api.Data.Repositories;

public interface IUserRepository
{
    Task<User?> UserGetBySecretAsync(Guid? domainId, Guid? userId, string? secret, CancellationToken cancellationToken = default);
}

internal class UserRepository(IDbReadonlyContext dbReadonlyContext) : IUserRepository
{
    private readonly IDbReadonlyContext _dbReadonlyContext = dbReadonlyContext;

    public async Task<User?> UserGetBySecretAsync(Guid? domainId, Guid? userId, string? secret, CancellationToken cancellationToken = default)
    {
        /*
         * honestly this validation isnt really needed as 
         * in theory the sql shouldnt return anything
         */
        if (domainId is null || domainId == Guid.Empty) return default;
        if (userId is null || userId == Guid.Empty) return default;
        if (string.IsNullOrEmpty(secret)) return default;

        const string sSql = @"
            SELECT u.[DomainId], u.[UserId], u.[CreatedDate], u.[CreatedBy], u.[ModifiedDate], u.[ModifiedBy], u.[UserName], u.[Email], d.[Name] AS Domain
            FROM [dbo].[User] u 
            JOIN [dbo].[Domain] d ON d.DomainId = u.DomainId
            JOIN [dbo].[UserSecret] us ON us.DomainId = u.DomainId AND us.UserId = u.UserId
            WHERE u.UserId = @UserId 
            AND u.DomainId = @DomainId
            AND us.Secret = @Secret;
        ";

        var param = new { DomainId = domainId, UserId = userId, Secret = secret };

        using var connection = _dbReadonlyContext.Connection;

        var sCmd = new CommandDefinition(sSql, param, cancellationToken: cancellationToken);

        var results = await connection.QueryAsync<User>(sCmd);

        return results.FirstOrDefault();
    }
}