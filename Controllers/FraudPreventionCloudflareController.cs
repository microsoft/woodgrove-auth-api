using System.Net;
using System.Text.Json;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using woodgroveapi.Helpers;
using woodgroveapi.Models;

namespace woodgroveapi.Controllers;

//[Authorize]
[ApiController]
[Route("[controller]")]
public class FraudPreventionCloudflareController : ControllerBase
{
    private readonly ILogger<FraudPreventionCloudflareController> _logger;
    private TelemetryClient _telemetry;
    private readonly IConfiguration _configuration;

    public FraudPreventionCloudflareController(ILogger<FraudPreventionCloudflareController> logger, TelemetryClient telemetry, IConfiguration configuration)
    {
        _logger = logger;
        _telemetry = telemetry;
        _configuration = configuration;
    }

    [HttpPost(Name = "FraudPreventionCloudflare")]
    public async Task<AttributeCollectionSubmitResponse> PostAsync([FromBody] AttributeCollectionRequest requestPayload)
    {
        //For Azure App Service with Easy Auth, validate the azp claim value
        // if (!AzureAppServiceClaimsHeader.Authorize(this.Request))
        // {
        //     AppInsightsHelper.TrackError("OnOtpSend", new Exception("Unauthorized"), this._telemetry, requestPayload.data);
        //     Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //     return null;
        // }

        // Track the page view 
        AppInsightsHelper.TrackApi("FraudPreventionCloudflare", this._telemetry, requestPayload.data);

        // Message object to return to Microsoft Entra ID
        AttributeCollectionSubmitResponse r = new AttributeCollectionSubmitResponse();

        // Check the input attributes and return a generic error message
        if (requestPayload.data.userSignUpInfo == null ||
            requestPayload.data.userSignUpInfo.attributes == null ||
            requestPayload.data.userSignUpInfo.attributes.specialDiet == null ||
            requestPayload.data.userSignUpInfo.attributes.specialDiet.value == null)
        {
            r.data.actions[0].odatatype = AttributeCollectionSubmitResponse_ActionTypes.ShowBlockPage;
            r.data.actions[0].message = "Can't find the Cloudflare code (specialDiet attribute is missing).";
            return r;
        }

        // Call the cloudflare validation endpoint
        string responseString = string.Empty;

        var nvc = new List<KeyValuePair<string, string>>();
        nvc.Add(new KeyValuePair<string, string>("secret", _configuration.GetSection("AppSettings:CloudflareSecret").Value!));
        nvc.Add(new KeyValuePair<string, string>("response", requestPayload.data.userSignUpInfo.attributes.specialDiet.value!));
        using var client = new HttpClient();
        using var req = new HttpRequestMessage(HttpMethod.Post, "https://challenges.cloudflare.com/turnstile/v0/siteverify") { Content = new FormUrlEncodedContent(nvc) };
        using var res = await client.SendAsync(req);

        responseString = await res.Content.ReadAsStringAsync();

        CloudflareResponse cloudflareResponse = CloudflareResponse.Parse(responseString);

        // Check the country name in on the supported list
        if (!cloudflareResponse.success)
        {
            r.data.actions[0].odatatype = AttributeCollectionSubmitResponse_ActionTypes.ShowValidationError;
            r.data.actions[0].message = "Please fix the following issues to proceed.";
            r.data.actions[0].attributeErrors = new AttributeCollectionSubmitResponse_AttributeError();
            r.data.actions[0].attributeErrors.specialDiet = $"Cloudflare: {responseString}";
            return r;
        }
        else
        {
            // No issues have been identified, proceed to create the account
            r.data.actions[0].odatatype = AttributeCollectionSubmitResponse_ActionTypes.ModifyAttributeValues;
            r.data.actions[0].attributes = new AttributeCollectionSubmitResponse_Attribute();

            // Remove the security token
            r.data.actions[0].attributes.specialDiet = "";
            return r;
        }
    }
}

public class CloudflareResponse
{
    public bool success { get; set; }

    public static CloudflareResponse Parse(string JsonString)
    {
        return JsonSerializer.Deserialize<CloudflareResponse>(JsonString);
    }
}