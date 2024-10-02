using System.Net;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using woodgroveapi.Helpers;
using woodgroveapi.Models;

namespace woodgroveapi.Controllers;

//[Authorize]
[ApiController]
[Route("[controller]")]
public class SignUpStartsTestController : ControllerBase
{
    private readonly ILogger<SignUpStartsTestController> _logger;
    private TelemetryClient _telemetry;
    private readonly IConfiguration _configuration;

    public SignUpStartsTestController(ILogger<SignUpStartsTestController> logger, TelemetryClient telemetry, IConfiguration configuration)
    {
        _logger = logger;
        _telemetry = telemetry;
        _configuration = configuration;
    }

    [HttpPost(Name = "SignUpStartsTest")]
    public AttributeCollectionStartResponse PostAsync([FromBody] AttributeCollectionRequest requestPayload)
    {
        //For Azure App Service with Easy Auth, validate the azp claim value
        // if (!AzureAppServiceClaimsHeader.Authorize(this.Request))
        // {
        //     AppInsightsHelper.TrackError("SignUpStartsTest", new Exception("Unauthorized"), this._telemetry, requestPayload.data);
        //     Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //     return null;
        // }

        var SimulateDelayInMiliSeconds = 0;
        int.TryParse(_configuration.GetSection("Demos:SimulateDelayMilliseconds").Value, out SimulateDelayInMiliSeconds);

        if (SimulateDelayInMiliSeconds > 0)
            Thread.Sleep(SimulateDelayInMiliSeconds);

        // Track the page view 
        AppInsightsHelper.TrackApi("SignUpStartsTest", this._telemetry, requestPayload.data);

        // Message object to return to Microsoft Entra ID
        AttributeCollectionStartResponse r = new AttributeCollectionStartResponse();
        r.data.actions[0].odatatype = AttributeCollectionStartResponse_ActionTypes.SetPrefillValues;
        r.data.actions[0].inputs = new AttributeCollectionStartResponse_Inputs();

        // Return the country and city
        r.data.actions[0].inputs.country = "es";
        // r.data.actions[0].inputs.city = "Madrind";

        // Return a promo code
        Random random = new Random();
        r.data.actions[0].inputs.promoCode = $"Promo code #{random.Next(1236, 9873)}";

        return r;
    }
}
