using System.Text.Json.Serialization;

namespace woodgroveapi.Models
{

    public class OnAttributeCollectionSubmitRequest
    {
        [JsonPropertyName("data")]
        public OnAttributeCollectionSubmitRequest_Data data { get; set; }
        public OnAttributeCollectionSubmitRequest()
        {
            data = new OnAttributeCollectionSubmitRequest_Data();
        }
    }

    public class OnAttributeCollectionSubmitRequest_Data
    {
        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }
        public string tenantId { get; set; }
        public string authenticationEventListenerId { get; set; }
        public string customAuthenticationExtensionId { get; set; }
        public AuthenticationContext authenticationContext { get; set; }
        public OnAttributeCollectionSubmitRequest_UserSignUpInfo userSignUpInfo { get; set; }

    }

    public class OnAttributeCollectionSubmitRequest_UserSignUpInfo
    {
        public OnAttributeCollectionSubmitRequest_BuiltInAttributes builtInAttributes { get; set; }
    }

    public class OnAttributeCollectionSubmitRequest_BuiltInAttributes
    {
        public string? country { get; set; }
        public string? city { get; set; }
    }
}