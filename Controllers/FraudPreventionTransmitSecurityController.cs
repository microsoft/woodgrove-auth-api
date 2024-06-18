using System.Net;
using System.Net.Http.Headers;
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
public class FraudPreventionTransmitSecurityController : ControllerBase
{
    private readonly ILogger<FraudPreventionTransmitSecurityController> _logger;
    private TelemetryClient _telemetry;
    private readonly IConfiguration _configuration;

    public FraudPreventionTransmitSecurityController(ILogger<FraudPreventionTransmitSecurityController> logger, TelemetryClient telemetry, IConfiguration configuration)
    {
        _logger = logger;
        _telemetry = telemetry;
        _configuration = configuration;
    }

    [HttpPost(Name = "FraudPreventionTransmitSecurity")]
    public async Task<AttributeCollectionSubmitResponse> PostAsync([FromBody] AttributeCollectionRequest? requestPayload)
    {

        //For Azure App Service with Easy Auth, validate the azp claim value
        // if (!AzureAppServiceClaimsHeader.Authorize(this.Request))
        // {
        //     AppInsightsHelper.TrackError("OnOtpSend", new Exception("Unauthorized"), this._telemetry, requestPayload.data);
        //     Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //     return null;
        // }

        // Track the page view 
        AppInsightsHelper.TrackApi("FraudPreventionTransmitSecurity", this._telemetry, requestPayload.data);

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

        string access_token = await AquireAccessTokneAsync();

        // Call the cloudflare validation endpoint
        string responseString = string.Empty;
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
        using var req = new HttpRequestMessage(HttpMethod.Get, $"https://api.transmitsecurity.io/risk/v1/recommendation?action_token={requestPayload.data.userSignUpInfo.attributes.specialDiet.value!}");
        using var res = await client.SendAsync(req);

        responseString = await res.Content.ReadAsStringAsync();

        TransmitSecurityResponse transmitSecurityResponse = TransmitSecurityResponse.Parse(responseString);

        // Check the recommendation type
        if (transmitSecurityResponse.recommendation != null && transmitSecurityResponse.recommendation.type != null &&
            (transmitSecurityResponse.recommendation.type == "ALLOW" || transmitSecurityResponse.recommendation.type == "TRUST"))
        {
            // No issues have been identified, proceed to create the account
            r.data.actions[0].odatatype = AttributeCollectionSubmitResponse_ActionTypes.ModifyAttributeValues;
            r.data.actions[0].attributes = new AttributeCollectionSubmitResponse_Attribute();

            // Remove the security token
            r.data.actions[0].attributes.specialDiet = "";
            return r;
        }
        else
        {
            r.data.actions[0].odatatype = AttributeCollectionSubmitResponse_ActionTypes.ShowValidationError;
            r.data.actions[0].message = "Account cannot be created due to a security issue";
            r.data.actions[0].attributeErrors = new AttributeCollectionSubmitResponse_AttributeError();

            if (transmitSecurityResponse.recommendation != null && transmitSecurityResponse.recommendation.type != null)
            {
                r.data.actions[0].attributeErrors.specialDiet = $"Recommendation: {transmitSecurityResponse.recommendation.type}";
            }
            else if (transmitSecurityResponse.message != null)
            {
                r.data.actions[0].attributeErrors.specialDiet = transmitSecurityResponse.message;
            }
            else
            {
                r.data.actions[0].attributeErrors.specialDiet = "Recommendation is null or empty";
            }

            return r;
        }
    }

    private async Task<string> AquireAccessTokneAsync()
    {
        var nvc = new List<KeyValuePair<string, string>>();
        nvc.Add(new KeyValuePair<string, string>("client_id", _configuration.GetSection("AppSettings:TransmitSecurity:AppId").Value!));
        nvc.Add(new KeyValuePair<string, string>("resource", "https://verify.identity.security"));
        nvc.Add(new KeyValuePair<string, string>("client_secret", _configuration.GetSection("AppSettings:TransmitSecurity:Secret").Value!));
        nvc.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
        using var client = new HttpClient();
        using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.transmitsecurity.io/oidc/token") { Content = new FormUrlEncodedContent(nvc) };
        using var res = await client.SendAsync(req);

        string responseString = await res.Content.ReadAsStringAsync();

        TransmitSecurityToken token = TransmitSecurityToken.Parse(responseString);
        return token.access_token;

    }
}

public class TransmitSecurityToken
{
    public string access_token { get; set; }
    public int expires_in { get; set; }
    public string token_type { get; set; }

    public static TransmitSecurityToken Parse(string JsonString)
    {
        return JsonSerializer.Deserialize<TransmitSecurityToken>(JsonString);
    }
}



public class TransmitSecurityResponse
{
    public string message { get; set; }
    public string id { get; set; }
    public long issued_at { get; set; }
    public Recommendation recommendation { get; set; }

    public static TransmitSecurityResponse Parse(string JsonString)
    {
        return JsonSerializer.Deserialize<TransmitSecurityResponse>(JsonString);
    }
}

public class Recommendation
{
    public string type { get; set; }
}