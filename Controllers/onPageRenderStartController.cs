using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using woodgroveapi.Models;

namespace woodgroveapi.Controllers;


//[Authorize]
[ApiController]
[Route("[controller]")]
public class onPageRenderStartController : ControllerBase
{
    private readonly ILogger<onPageRenderStartController> _logger;

    public onPageRenderStartController(ILogger<onPageRenderStartController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "onPageRenderStart")]
    public PageRenderStartResponse PostAsync([FromBody] PageRenderStartRequest requestPayload)
    {
        //For Azure App Service with Easy Auth, validate the azp claim value
        //if (!AzureAppServiceClaimsHeader.Authorize(this.Request))
        //{
        //     r.StatusCode = (int)HttpStatusCode.Unauthorized;
        //     return null;
        //}

        PageRenderStartResponse r = new PageRenderStartResponse();
        r.type =  requestPayload.type;
        r.source = requestPayload.source;

        r.data.actions[0].tenantBranding.signInPageText = "This is my first test";
        r.data.actions[0].tenantBranding.customTermsOfUseText = "My TOS test";


        return r;
    }
}