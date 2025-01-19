using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add Azure stream log service
builder.Logging.AddAzureWebAppDiagnostics();
builder.Services.Configure<AzureFileLoggerOptions>(options =>
{
    options.FileName = "azure-diagnostics-";
    options.FileSizeLimit = 50 * 1024;
    options.RetainedFileCountLimit = 5;
});
builder.Logging.AddFilter((provider, category, logLevel) =>
{
    return provider!.ToLower().Contains("woodgroveapi");
});

ConfigurationSection entraExternalIdCustomAuthTokenSettings = (ConfigurationSection)builder.Configuration.GetSection("EntraExternalIdCustomAuthToken");

// Reference: 
// There is an issue validating the first party token with 
// https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.authentication.jwtbearer
// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/configure-jwt-bearer-authentication
builder.Services.AddAuthentication()
    .AddJwtBearer("EntraExternalIdCustomAuthToken", jwtOptions =>
    {
        jwtOptions.MetadataAddress = entraExternalIdCustomAuthTokenSettings["MetadataAddress"]!;
        jwtOptions.Audience = entraExternalIdCustomAuthTokenSettings["Audience"];
        jwtOptions.IncludeErrorDetails = true;
        jwtOptions.MapInboundClaims = false;
        jwtOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true
        };
        jwtOptions.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                // Validate the authorized party (the app who issued the token)
                string? clientappId = context?.Principal?.Claims.FirstOrDefault(x => x.Type == "azp" && x.Value == "99045fe1-7639-4a75-9d4a-577b6ca3810f")?.Value;
                if (clientappId == null)
                {
                    context!.Fail("Invalid azp claim value");
                }
                return Task.CompletedTask;
            }
        };
    });


ConfigurationSection entraExternalIdUserToken = (ConfigurationSection)builder.Configuration.GetSection("EntraExternalIdUserToken");
builder.Services.AddAuthentication()
    .AddJwtBearer("EntraExternalIdUserToken", jwtOptions =>
    {
        jwtOptions.MetadataAddress = entraExternalIdUserToken["MetadataAddress"]!;
        jwtOptions.Audience = entraExternalIdUserToken["Audience"];
        jwtOptions.IncludeErrorDetails = true;
        jwtOptions.MapInboundClaims = false;
        jwtOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true
        };
    });

// Add in memory cache                                                  
builder.Services.AddMemoryCache();

builder.Services.AddControllers();

// The following line enables Application Insights telemetry collection.
builder.Services.AddApplicationInsightsTelemetry();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Woodgrove custom authentication extension API",
        Description = "This dotnet Web API endpoint demonstrate how to use Microsoft Entra External ID's custom authentication extension for various events. Checkout the [source code](https://github.com/microsoft/woodgrove-api) and [request samples](./help.html) <br> <br> Assembly version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version!.ToString(),
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
