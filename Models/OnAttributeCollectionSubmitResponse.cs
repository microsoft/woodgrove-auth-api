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
            data.odatatype = "microsoft.graph.onOnAttributeCollectionSubmitResponseData";

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
        public List<OnAttributeCollectionSubmitResponse_AttributeError> attributeErrors { get; set; }

        public OnAttributeCollectionSubmitResponse_Action()
        {
            odatatype = "microsoft.graph.OnAttributeCollectionSubmitResponse_Data";
        }
    }

    public class OnAttributeCollectionSubmitResponse_AttributeError
    {
        public string name { get; set; }
        public string value { get; set; }

        public OnAttributeCollectionSubmitResponse_AttributeError(string Name, string Value)
        {
            this.name = Name;
            this.value = Value;
        }
    }

    public class OnAttributeCollectionSubmitResponse_ActionTypes
    {
        public const string ShowValidationError = "microsoft.graph.ShowValidationError";
        public const string ContinueWithDefaultBehavior = "microsoft.graph.continueWithDefaultBehavior";
        public const string ModifyAttributeValues = "microsoft.graph.modifyAttributeValues";
        public const string ShowBlockPage = "microsoft.graph.showBlockPage";
    }
}