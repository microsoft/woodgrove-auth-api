using System.Text.Json.Serialization;

namespace woodgroveapi.Models
{

    public class OnAttributeCollectionSubmitResponse
    {
        [JsonPropertyName("data")]
        public OnAttributeCollectionSubmitResponse_Data data { get; set; }
        public OnAttributeCollectionSubmitResponse()
        {
            data = new OnAttributeCollectionSubmitResponse_Data();
            data.odatatype = "microsoft.graph.onAttributeCollectionSubmitResponseData";

            this.data.actions = new List<OnAttributeCollectionSubmitResponse_Action>();
            this.data.actions.Add(new OnAttributeCollectionSubmitResponse_Action());
        }
    }

    public class OnAttributeCollectionSubmitResponse_Data
    {
        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }
        public List<OnAttributeCollectionSubmitResponse_Action> actions { get; set; }
    }

    public class OnAttributeCollectionSubmitResponse_Action
    {
        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public OnAttributeCollectionSubmitResponse_AttributeError attributeErrors { get; set; }

        public OnAttributeCollectionSubmitResponse_Action()
        {
            odatatype = "microsoft.graph.OnAttributeCollectionSubmitResponse_Data";
        }
    }

    public class OnAttributeCollectionSubmitResponse_AttributeError
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? city { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? country { get; set; }
    }

    public class OnAttributeCollectionSubmitResponse_ActionTypes
    {
        public const string ShowValidationError = "microsoft.graph.attributeCollectionSubmit.showValidationError";
        public const string ContinueWithDefaultBehavior = "microsoft.graph.attributeCollectionSubmit.continueWithDefaultBehavior";
        public const string ModifyAttributeValues = "microsoft.graph.attributeCollectionSubmit.modifyAttributeValues";
        public const string ShowBlockPage = "microsoft.graph.attributeCollectionSubmit.showBlockPage";
    }
}