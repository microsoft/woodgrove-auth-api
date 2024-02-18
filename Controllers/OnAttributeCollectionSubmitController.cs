using System.Net;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using woodgroveapi.Helpers;
using woodgroveapi.Models;

namespace woodgroveapi.Controllers;

//[Authorize]
[ApiController]
[Route("[controller]")]
public class OnAttributeCollectionSubmitController : ControllerBase
{
    private readonly ILogger<OnAttributeCollectionSubmitController> _logger;
    private TelemetryClient _telemetry;

    public OnAttributeCollectionSubmitController(ILogger<OnAttributeCollectionSubmitController> logger, TelemetryClient telemetry)
    {
        _logger = logger;
        _telemetry = telemetry;
    }

    [HttpPost(Name = "OnAttributeCollectionSubmit")]
    public AttributeCollectionSubmitResponse PostAsync([FromBody] AttributeCollectionRequest requestPayload)
    {
        // For Azure App Service with Easy Auth, validate the azp claim value
        // if (!AzureAppServiceClaimsHeader.Authorize(this.Request))
        // {
        //     Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //     return null;
        // }

        // Track the page view 
        AppInsightsHelper.TrackApi("OnAttributeCollectionSubmit", this._telemetry, requestPayload.data);

        // List of countries and cities where Woodgrove operates
        Dictionary<string, string> CountriesList = new Dictionary<string, string>();
        CountriesList.Add("au", " Sydney, Brisbane, Melbourne");
        CountriesList.Add("es", " Madrid, Barcelona, Seville");
        CountriesList.Add("us", " New York, Chicago, Boston, Seattle");

        // Message object to return to Azure AD
        AttributeCollectionSubmitResponse r = new AttributeCollectionSubmitResponse();

        // Check the input attributes and return a generic error message
        if (requestPayload.data.userSignUpInfo == null ||
            requestPayload.data.userSignUpInfo.attributes == null ||
            requestPayload.data.userSignUpInfo.attributes.country == null ||
            requestPayload.data.userSignUpInfo.attributes.country.value == null ||
            requestPayload.data.userSignUpInfo.attributes.city == null ||
            requestPayload.data.userSignUpInfo.attributes.city.value == null)
        {
            r.data.actions[0].odatatype = AttributeCollectionSubmitResponse_ActionTypes.ShowBlockPage;
            r.data.actions[0].message = "Can't find the country and/or city attributes.";
            return r;
        }

        // Demonstrates the use of block response
        if (requestPayload.data.userSignUpInfo.attributes.city.value.ToLower() == "block")
        {
            r.data.actions[0].odatatype = AttributeCollectionSubmitResponse_ActionTypes.ShowBlockPage;
            r.data.actions[0].message = "You can't create an account with 'block' city.";
            return r;
        }

        // Check the country name in on the supported list
        if (!CountriesList.ContainsKey(requestPayload.data.userSignUpInfo.attributes.country.value))
        {
            r.data.actions[0].odatatype = AttributeCollectionSubmitResponse_ActionTypes.ShowValidationError;
            r.data.actions[0].message = "Please fix the following issues to proceed.";
            r.data.actions[0].attributeErrors = new AttributeCollectionSubmitResponse_AttributeError();
            r.data.actions[0].attributeErrors.country = $"We don't operate in '{requestPayload.data.userSignUpInfo.attributes.country.value}'";
            return r;
        }

        // Get the countries' cities
        string cities = CountriesList[requestPayload.data.userSignUpInfo.attributes.country.value];

        // Check if the city provided by user in the supported list
        if (!(cities + ",").ToLower().Contains($" {requestPayload.data.userSignUpInfo.attributes.city.value.ToLower()},"))
        {
            r.data.actions[0].odatatype = AttributeCollectionSubmitResponse_ActionTypes.ShowValidationError;
            r.data.actions[0].message = "Please fix the following issues to proceed.";
            r.data.actions[0].attributeErrors = new AttributeCollectionSubmitResponse_AttributeError();
            r.data.actions[0].attributeErrors.city = $"We don't operate in this city. Please select one of the following:{cities}";
        }
        else
        {
            // No issues have been identified, proceed to create the account
            r.data.actions[0].odatatype = AttributeCollectionSubmitResponse_ActionTypes.ContinueWithDefaultBehavior;
        }

        return r;
    }
}
