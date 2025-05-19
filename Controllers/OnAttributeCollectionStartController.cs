using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using woodgroveapi.Helpers;
using woodgroveapi.Models;

namespace woodgroveapi.Controllers;

//[Authorize]
[ApiController]
[Route("[controller]")]
public class OnAttributeCollectionStartController : ControllerBase
{
    private readonly ILogger<OnAttributeCollectionStartController> _logger;
    private TelemetryClient _telemetry;

    public OnAttributeCollectionStartController(ILogger<OnAttributeCollectionStartController> logger, TelemetryClient telemetry)
    {
        _logger = logger;
        _telemetry = telemetry;
    }

    [HttpPost(Name = "OnAttributeCollectionStart")]
    public IActionResult PostAsync([FromBody] AttributeCollectionRequest requestPayload)
    {
        if (requestPayload == null || requestPayload.data == null)
        {
            _logger.LogWarning("Invalid request payload received in OnAttributeCollectionStart.");
            return BadRequest(new { error = "Request payload or data is null." });
        }

        // For Azure App Service with Easy Auth, validate the azp claim value
        // if (!AzureAppServiceClaimsHeader.Authorize(this.Request))
        // {
        //     Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //     return null;
        // }

        // Track the page view 
        AppInsightsHelper.TrackApi("OnAttributeCollectionStart", this._telemetry, requestPayload.data);

        // Message object to return to Microsoft Entra ID
        AttributeCollectionStartResponse r = new AttributeCollectionStartResponse();
        r.data.actions[0].odatatype = AttributeCollectionStartResponse_ActionTypes.SetPrefillValues;
        r.data.actions[0].inputs = new AttributeCollectionStartResponse_Inputs();

        // Return the country and city
        r.data.actions[0].inputs.country = "es";
        // r.data.actions[0].inputs.city = "Madrind";

        // Return a promo code
        Random random = new Random();
        r.data.actions[0].inputs.promoCode = $"#{random.Next(1236, 9873)}";

        return Ok(r);
    }
}