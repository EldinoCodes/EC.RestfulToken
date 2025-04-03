using Microsoft.Data.SqlClient;
using System.Data;

namespace EC.RestfulToken.Server.Api.Data.Contexts;

public interface IDbReadonlyContext
{
    public IDbConnection Connection { get; }
}

internal class DbReadonlyContext(IConfiguration configuration) : IDbReadonlyContext
{
    private readonly string _connectionString = configuration.GetConnectionString("dbReadonly") ?? throw new ArgumentNullException("invalid connectionstring");
    public IDbConnection Connection => new SqlConnection(_connectionString);
}