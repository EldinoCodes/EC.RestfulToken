
using EC.RestfulToken.Server.Api.Data.Contexts;
using EC.RestfulToken.Server.Api.Data.Repositories;
using EC.RestfulToken.Server.Api.Services;
using EC.RestfulToken.Server.Api.Swagger;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EC.RestfulToken.Server.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        /*
         * add auth to swagger, this will add a lock icon to the swagger ui
         */
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, CustomSwaggerGenOptions>();

        /*
         * check out this simple jwt auth implementation, 
         * force auth on all endpoints unless [AllowAnonymous] is used
         */
        builder.Services.AddRestfulTokenApiAuthentication(builder.Configuration);

        /*
         * added db context, user repository, and a 
         * simple auth service to generate jwt tokens
         */
        builder.Services.AddScoped<IDbReadonlyContext, DbReadonlyContext>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IAuthService, AuthService>();

        // added simple test content service to return some data
        builder.Services.AddScoped<ITestContentService, TestContentService>();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
