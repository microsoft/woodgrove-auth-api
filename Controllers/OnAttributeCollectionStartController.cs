// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using woodgroveapi.Models.Request;
// using woodgroveapi.Models.Response;

// namespace woodgroveapi.Controllers;

// [Authorize]
// [ApiController]
// [Route("[controller]")]
// public class OnAttributeCollectionStartController : ControllerBase
// {
//     private readonly ILogger<OnAttributeCollectionStartController> _logger;

//     public OnAttributeCollectionStartController(ILogger<OnAttributeCollectionStartController> logger)
//     {
//         _logger = logger;
//     }

//     [HttpPost(Name = "OnAttributeCollectionStart")]
//     public ResponsePayload PostAsync([FromBody] RequestPayload _)
//     {
//         _logger.LogInformation("*********** OnAttributeCollectionStart ***********");

//         // Read the correlation ID from the Azure AD  request    
//         //string correlationId = data.data.authenticationContext.correlationId; ;

//         // Claims to return to Azure AD
//         ResponsePayload r = new ResponsePayload(ResponseType.OnAttributeCollectionStartResponseData);
//         r.AddAction(ActionType.SetPrefillValues);
//         r.data.actions[0].inputs.jobTitle = "This is my test";
//         return r;
//     }
// }
