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
        public OnAttributeCollectionSubmitRequest_attributes attributes { get; set; }
    }

    public class OnAttributeCollectionSubmitRequest_attributes
    {
        public OnAttributeCollectionSubmitRequest_Email? email { get; set; }
        public OnAttributeCollectionSubmitRequest_City? city { get; set; }
        public OnAttributeCollectionSubmitRequest_Country? country { get; set; }
        public OnAttributeCollectionSubmitRequest_DisplayName? displayName { get; set; }
    }

    public class OnAttributeCollectionSubmitRequest_City
    {
        public string value { get; set; }

        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }
        public string attributeType { get; set; }
    }

    public class OnAttributeCollectionSubmitRequest_Country
    {
        public string value { get; set; }

        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }
        public string attributeType { get; set; }
    }

    public class OnAttributeCollectionSubmitRequest_DisplayName
    {
        public string value { get; set; }

        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }
        public string attributeType { get; set; }
    }

    public class OnAttributeCollectionSubmitRequest_Email
    {
        public string value { get; set; }

        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }
        public string attributeType { get; set; }
    }
}