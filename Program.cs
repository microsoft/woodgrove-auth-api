using Microsoft.Identity.Web;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.OpenApi.Models;

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
    return provider.ToLower().Contains("woodgroveapi");
});

// Disable the default claims mapping.
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

// Add the authentication services.
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

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
        Title = "Woodgrove demo API",
        Description = "This dotnet Web API endpoint demonstrate how to use Microsoft Entra External ID's custom authentication extension for various events. Checkout the [source code](https://github.com/microsoft/woodgrove-api) and [request samples](./help.html) <br> <br> Assembly version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
    });
});

// Verifies that the caller of the Web API is always the Microsoft Entra External ID.
// string policyName = "VerifyCallerIsCiamSts";
// builder.Services.AddAuthorization(options => {
//     options.AddPolicy(policyName, builder =>
//     {
//         // For more information, https://learn.microsoft.com/azure/active-directory/develop/custom-extension-overview#protect-your-rest-api
//         builder.RequireClaim("azp", "99045fe1-7639-4a75-9d4a-577b6ca3810f");
//     });
//     options.DefaultPolicy = options.GetPolicy(policyName)!;
// });

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
