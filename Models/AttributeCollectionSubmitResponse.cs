using System.Text.Json.Serialization;

namespace woodgroveapi.Models
{

    public class AttributeCollectionSubmitResponse
    {
        [JsonPropertyName("data")]
        public AttributeCollectionSubmitResponse_Data data { get; set; }
        public AttributeCollectionSubmitResponse()
        {
            data = new AttributeCollectionSubmitResponse_Data();
            data.odatatype = "microsoft.graph.onAttributeCollectionSubmitResponseData";

            this.data.actions = new List<AttributeCollectionSubmitResponse_Action>();
            this.data.actions.Add(new AttributeCollectionSubmitResponse_Action());
        }
    }

    public class AttributeCollectionSubmitResponse_Data
    {
        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }
        public List<AttributeCollectionSubmitResponse_Action> actions { get; set; }
    }

    public class AttributeCollectionSubmitResponse_Action
    {
        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AttributeCollectionSubmitResponse_Attribute attributes { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AttributeCollectionSubmitResponse_AttributeError attributeErrors { get; set; }
    }

    public class AttributeCollectionSubmitResponse_Attribute
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("preferredLanguage")]
        public string? PreferredLanguage { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("extension_0cae61cc83e94edd978ec2fde3c5f2f3_SpecialDiet")]
        public string? SpecialDiet { get; set; }
    }
    public class AttributeCollectionSubmitResponse_AttributeError
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? city { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? country { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("extension_0cae61cc83e94edd978ec2fde3c5f2f3_SpecialDiet")]
        public string? specialDiet { get; set; }
    }

    public class AttributeCollectionSubmitResponse_ActionTypes
    {
        public const string ShowValidationError = "microsoft.graph.attributeCollectionSubmit.showValidationError";
        public const string ContinueWithDefaultBehavior = "microsoft.graph.attributeCollectionSubmit.continueWithDefaultBehavior";
        public const string ModifyAttributeValues = "microsoft.graph.attributeCollectionSubmit.modifyAttributeValues";
        public const string ShowBlockPage = "microsoft.graph.attributeCollectionSubmit.showBlockPage";
    }
}