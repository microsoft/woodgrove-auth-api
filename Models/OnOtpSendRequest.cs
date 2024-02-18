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

    public class OnOtpSendRequest_Data
    {
        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }
        public string tenantId { get; set; }
        public string authenticationEventListenerId { get; set; }
        public string customAuthenticationExtensionId { get; set; }
        public AuthenticationContext authenticationContext { get; set; }
        public OtpContext otpContext { get; set; }
    }

    public class OtpContext
    {
        public string identifier { get; set; }
         public string onetimecode { get; set; }
    }
}