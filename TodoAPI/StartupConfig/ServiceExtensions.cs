using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using TodoLibrary.DataAccess;

namespace TodoAPI.StartupConfig;

public static class ServiceExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
       builder.Services.AddControllers();
       builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.AddAuthServices();
        builder.AddHealthCheckServices();
        builder.AddCustomServices();

    }
    public static void AddAuthServices(this WebApplicationBuilder builder)
    {
        
        builder.Services.AddAuthorization(opts =>
        {
            opts.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        });

        builder.Services.AddAuthentication("Bearer").AddJwtBearer(opts =>
        {
            opts.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
                ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Authentication:SecretKey")))
            };
        });
    }
    public static void AddHealthCheckServices(this WebApplicationBuilder builder)
    {
        // checks sql server
        builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("DBStorageString"));
    }
    public static void AddCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
        builder.Services.AddSingleton<ITodoData, TodoData>();
    }
}