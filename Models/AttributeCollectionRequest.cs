using System.Text.Json.Serialization;

namespace woodgroveapi.Models
{

    public class AttributeCollectionRequest
    {
        [JsonPropertyName("data")]
        public AttributeCollectionRequest_Data data { get; set; }
        public AttributeCollectionRequest()
        {
            data = new AttributeCollectionRequest_Data();
        }
    }

    public class AttributeCollectionRequest_Data
    {
        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }
        public string tenantId { get; set; }
        public string authenticationEventListenerId { get; set; }
        public string customAuthenticationExtensionId { get; set; }
        public AuthenticationContext authenticationContext { get; set; }
        public UserSignUpInfo userSignUpInfo { get; set; }

    }
}