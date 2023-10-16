using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using woodgroveapi.Models;

namespace woodgroveapi.Controllers;

//[Authorize]
[ApiController]
[Route("[controller]")]
public class OnAttributeCollectionStartController : ControllerBase
{
    private readonly ILogger<OnAttributeCollectionStartController> _logger;

    public OnAttributeCollectionStartController(ILogger<OnAttributeCollectionStartController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "OnAttributeCollectionStart")]
    public AttributeCollectionStartResponse PostAsync([FromBody] AttributeCollectionRequest requestPayload)
    {
        // For Azure App Service with Easy Auth, validate the azp claim value
        // if (!AzureAppServiceClaimsHeader.Authorize(this.Request))
        // {
        //     Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //     return null;
        // }

        // Message object to return to Azure AD
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
