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
        r.type = requestPayload.type;
        r.source = requestPayload.source;

        var branding = r.data.actions[0].tenantBranding;
        string appUrl = "";
        string welcome = "";

        switch (requestPayload.data.authenticationContext.clientServicePrincipal.appId)
        {
            case "7a30b8ed-42a3-4d1e-89ad-14d4ca3c9a52":
                appUrl = "https://woodgrovebanking.com";
                welcome = "**Woodgrove online bank**";
                break;

            case "65d59577-c9d1-485b-87a5-80b92a99fbfa":
                appUrl = "https://woodgroverestaurants.com";
                welcome = "**Woodgrove restaurant**";
                break;

            default:
                appUrl = "https://woodgrovedemo.com";
                welcome = "**Woodgrove groceries** online store";
                break;
        }

        r.data.actions[0].tenantBranding = RetrieveBranding(appUrl, welcome);
        return r;
    }

    private PageRenderStartResponse_TenantBranding RetrieveBranding(string appUrl, string welcome)
    {
        PageRenderStartResponse_TenantBranding branding = new PageRenderStartResponse_TenantBranding();

        branding.customCSS = $"{appUrl}/Company-branding/customcss.css";
        branding.backgroundImage = $"{appUrl}/Company-branding/background.jpeg";
        branding.favicon = $"{appUrl}/Company-branding/favicon.png";

        // Header
        branding.loginPageLayoutConfiguration = new PageRenderStartResponse_LoginPageLayoutConfiguration();
        branding.loginPageLayoutConfiguration.isHeaderShown = true;
        branding.loginPageLayoutConfiguration.isFooterShown = true;
        branding.headerLogo = $"{appUrl}/Company-branding/headerlogo.png";

        // Sign in box
        branding.signInPageText = $"Welcome to {welcome}. Sign-in with your credentials, or create a new account. You can also sign-in with your *social accounts*, such as Facebook or Google. For help, please [contact us](https://woodgrovedemo.com/help).";
        branding.bannerLogo = $"{appUrl}/Company-branding/bannerlogo.png";

        // Terms of use
        branding.customTermsOfUseText = "Woodgrove terms of use";
        branding.customTermsOfUseUrl = $"{appUrl}/tos";

        // Privacy & Cookies statement
        branding.customPrivacyAndCookiesText = "Privacy & Cookies statement";
        branding.customPrivacyAndCookiesUrl = $"{appUrl}/privacy";

        //branding.contentCustomization = new PageRenderStartResponse_ContentCustomization();
        // branding.contentCustomization.attributeCollection= new PageRenderStartResponse_AttributeCollection();
        // branding.contentCustomization.attributeCollection.signIn_Description = "This is my test";
        // branding.contentCustomization.attributeCollection.signIn_Title = "This is my test";


        //branding.contentCustomization.attributeCollection = "[{\"key\": \"SignIn_Description\", \"value\": \"This is my test\" },  {  \"key\": \"SignIn_Title\", \"value\": \"This is my test\" }]";



        // branding.contentCustomization.attributeCollection = new List<PageRenderStartResponse_AttributeCollection>();
        // branding.contentCustomization.attributeCollection.Add( new PageRenderStartResponse_AttributeCollection("SignIn_Description", "This is my test"));
        // branding.contentCustomization.attributeCollection.Add( new PageRenderStartResponse_AttributeCollection("SignIn_Title", "This is my test"));

        return branding;

    }
}