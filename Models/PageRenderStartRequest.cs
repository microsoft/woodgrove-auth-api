using System.Text.Json.Serialization;

namespace woodgroveapi.Models
{

    public class PageRenderStartRequest
    {
        [JsonPropertyName("data")]
        public PageRenderStartRequest_Data data { get; set; }

        public string type { get; set; }

        public string source { get; set; }

        // Constructor
        public PageRenderStartRequest()
        {
            data = new PageRenderStartRequest_Data();
        }
    }

    public class PageRenderStartRequest_Data
    {
        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }
        public string pageId { get; set; }
        public string tenantId { get; set; }
        public string authenticationEventListenerId { get; set; }
        public string customAuthenticationExtensionId { get; set; }
        public AuthenticationContext authenticationContext { get; set; }

    }
}