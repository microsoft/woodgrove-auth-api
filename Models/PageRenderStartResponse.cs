using System.Data.Common;
using System.Text.Json.Serialization;

namespace woodgroveapi.Models
{


    public class PageRenderStartResponse
    {
        public string type { get; set; }
        public string source { get; set; }
        public PageRenderStartResponse_Data data { get; set; }

        public PageRenderStartResponse()
        {
            data = new PageRenderStartResponse_Data();

            this.data.actions = new List<PageRenderStartResponse_Action>();
            this.data.actions.Add(new PageRenderStartResponse_Action());
        }
    }

    public class PageRenderStartResponse_Data
    {
        public List<PageRenderStartResponse_Action> actions { get; set; }
    }

    public class PageRenderStartResponse_Action
    {
        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }
        public PageRenderStartResponse_TenantBranding tenantBranding { get; set; }

        public PageRenderStartResponse_Action()
        {
            odatatype = "microsoft.graph.PageRenderStart.OverrideBranding";
            tenantBranding = new PageRenderStartResponse_TenantBranding();
        }
    }

    public class PageRenderStartResponse_ContentCustomization
    {
        public PageRenderStartResponse_ContentCustomization() { }

        public List<PageRenderStartResponse_AttributeCollection> attributeCollection { get; set; }

        // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        // public PageRenderStartResponse_AttributeCollection attributeCollection { get; set; }


        // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        // public string attributeCollection { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string adminConsent { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string registrationCampaign { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string conditionalAccess { get; set; }
    }

    // public class PageRenderStartResponse_AttributeCollection
    // {
    //     public string signIn_Description { get; set; }
    //     public string signIn_Title { get; set; }
    // }

    public class PageRenderStartResponse_AttributeCollection
    {
        public PageRenderStartResponse_AttributeCollection()
        {

        }

        public PageRenderStartResponse_AttributeCollection(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
        public string key { get; set; }
        public string value { get; set; }
    }

    public class PageRenderStartResponse_LoginPageLayoutConfiguration
    {
        public bool isHeaderShown { get; set; }
        public int layoutTemplateType { get; set; }
        public bool isFooterShown { get; set; }
    }

    public class PageRenderStartResponse_LoginPageTextVisibilitySettings
    {
        public bool hideCannotAccessYourAccount { get; set; }
        public bool hideForgotMyPassword { get; set; }
        public bool hideTermsOfUse { get; set; }
        public bool hidePrivacyAndCookies { get; set; }
    }

    public class PageRenderStartResponse_TenantBranding
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string customAccountResetCredentialsUrl { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string customCannotAccessYourAccountText { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string customForgotMyPasswordText { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string backgroundColor { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string bannerLogo { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string signInPageText { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string customCSS { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string customPrivacyAndCookiesUrl { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string customPrivacyAndCookiesText { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string customTermsOfUseUrl { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string customTermsOfUseText { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string backgroundImage { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string headerLogo { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PageRenderStartResponse_LoginPageLayoutConfiguration loginPageLayoutConfiguration { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PageRenderStartResponse_ContentCustomization contentCustomization { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PageRenderStartResponse_LoginPageTextVisibilitySettings loginPageTextVisibilitySettings { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string favicon { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string squareLogoDark { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string squareLogo { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string usernameHintText { get; set; }
    }


}