using System.Data.Common;
using System.Text.Json.Serialization;

namespace woodgroveapi.Models
{

    public class OnOtpSendResponse
    {
        public OnOtpSendResponse_Data data { get; set; }

        public OnOtpSendResponse()
        {
            data = new OnOtpSendResponse_Data();

            this.data.actions = new List<OnOtpSendResponse_Action>();
            this.data.actions.Add(new OnOtpSendResponse_Action());
        }
    }

    public class OnOtpSendResponse_Data
    {
        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }
        public List<OnOtpSendResponse_Action> actions { get; set; }

        public OnOtpSendResponse_Data()
        {
            this.odatatype = "microsoft.graph.OnOtpSendResponseData";
        }
    }

    public class OnOtpSendResponse_Action
    {
        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }

        public OnOtpSendResponse_Action()
        {
            odatatype = "microsoft.graph.OtpSend.continueWithDefaultBehavior";
        }
    }
}