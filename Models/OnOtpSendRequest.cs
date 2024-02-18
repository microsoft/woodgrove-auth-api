using System.Text.Json.Serialization;

namespace woodgroveapi.Models
{

    public class OnOtpSendRequest
    {
        [JsonPropertyName("data")]
        public OnOtpSendRequest_Data data { get; set; }
        public OnOtpSendRequest()
        {
            data = new OnOtpSendRequest_Data();
        }
    }

    public class OnOtpSendRequest_Data: AllRequestData
    {
        public OtpContext otpContext { get; set; }
    }

    public class OtpContext
    {
        public string identifier { get; set; }
         public string onetimecode { get; set; }
    }
}