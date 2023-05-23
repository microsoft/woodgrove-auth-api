using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using woodgroveapi.Models;

namespace woodgroveapi.Controllers;


//[Authorize]
[ApiController]
[Route("[controller]")]
public class TokenIssuanceStartController : ControllerBase
{
    private readonly ILogger<TokenIssuanceStartController> _logger;

    public TokenIssuanceStartController(ILogger<TokenIssuanceStartController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "TokenIssuanceStart")]
    public TokenIssuanceStartResponse PostAsync([FromBody] TokenIssuanceStartRequest requestPayload)
    {

        //For Azure App Service with Easy Auth, validate the azp claim value
        //if (!AzureAppServiceClaimsHeader.Authorize(this.Request))
        //{
        //     Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //     return null;
        //}

        // Read the correlation ID from the Azure AD  request    
        string correlationId = requestPayload.data.authenticationContext.correlationId; ;

        // Claims to return to Azure AD
        TokenIssuanceStartResponse r = new TokenIssuanceStartResponse();
        r.data.actions[0].claims.CorrelationId = correlationId;
        r.data.actions[0].claims.ApiVersion = "1.0.3";

        // Loyalty program data
        Random random = new Random();
        string[] tiers = { "Silver", "Gold", "Platinum", "Diamond" };
        r.data.actions[0].claims.LoyaltyNumber = random.Next(123467, 999989).ToString();
        r.data.actions[0].claims.LoyaltySince = DateTime.Now.AddDays((-1) * random.Next(30, 365)).ToString("dd MMMM yyyy");
        r.data.actions[0].claims.LoyaltyTier = tiers[random.Next(0, tiers.Length)];

        // Custom roles
        r.data.actions[0].claims.CustomRoles.Add("Writer");
        r.data.actions[0].claims.CustomRoles.Add("Editor");
        return r;
    }
}
