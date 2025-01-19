using System.Net;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using woodgroveapi.Helpers;
using woodgroveapi.Models;

namespace woodgroveapi.Controllers;


//[Authorize]
[ApiController]
[Route("[controller]")]
public class OnTokenIssuanceStartController : ControllerBase
{
    private readonly ILogger<OnTokenIssuanceStartController> _logger;
    private TelemetryClient _telemetry;
    private readonly IMemoryCache _memoryCache;

    public OnTokenIssuanceStartController(ILogger<OnTokenIssuanceStartController> logger, IMemoryCache memoryCache, TelemetryClient telemetry)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _telemetry = telemetry;
    }

    [HttpPost(Name = "OnTokenIssuanceStart")]
    public TokenIssuanceStartResponse PostAsync([FromBody] TokenIssuanceStartRequest requestPayload)
    {
        // Track the page view 
        AppInsightsHelper.TrackApi("OnTokenIssuanceStart", this._telemetry, requestPayload.data);

        //For Azure App Service with Easy Auth, validate the azp claim value
        //if (!AzureAppServiceClaimsHeader.Authorize(this.Request))
        //{
        //     Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //     return null;
        //}

        // Read the correlation ID from the Microsoft Entra ID  request    
        string correlationId = requestPayload.data.authenticationContext.correlationId; ;

        // Claims to return to Microsoft Entra ID
        TokenIssuanceStartResponse r = new TokenIssuanceStartResponse();
        r.data.actions[0].claims.CorrelationId = correlationId;
        r.data.actions[0].claims.ApiVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version!.ToString();

        // Loyalty program data
        Random random = new Random();
        string[] tiers = { "Silver", "Gold", "Platinum", "Diamond" };
        r.data.actions[0].claims.LoyaltyNumber = random.Next(123467, 999989).ToString();
        r.data.actions[0].claims.LoyaltySince = DateTime.Now.AddDays((-1) * random.Next(30, 365)).ToString("dd MMMM yyyy");
        r.data.actions[0].claims.LoyaltyTier = tiers[random.Next(0, tiers.Length)];

        // Custom roles
        r.data.actions[0].claims.CustomRoles.Add("Writer");
        r.data.actions[0].claims.CustomRoles.Add("Editor");


        // Try to get the cache object for the current user
        string userId = requestPayload.data.authenticationContext.user!.id!;

        if (_memoryCache.TryGetValue(userId, out ActAsEntity actAsEntity))
        {
            r.data.actions[0].claims.ActAs = actAsEntity!.ActAs;

            // Remove the user's data from the cache
            _memoryCache.Remove(userId);
        }
        return r;
    }
}
