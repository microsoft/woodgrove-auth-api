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

    public class PageRenderStartRequest_Data: AllRequestData
    {
        public string pageId { get; set; }
    }
}