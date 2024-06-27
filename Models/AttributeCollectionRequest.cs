using System.Text.Json;
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

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    public class AttributeCollectionRequest_Data : AllRequestData
    {
        public UserSignUpInfo userSignUpInfo { get; set; }

    }
}