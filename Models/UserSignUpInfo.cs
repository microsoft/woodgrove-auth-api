using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace woodgroveapi.Models
{

    public class UserSignUpInfo
    {
        public UserSignUpInfo_Attributes attributes { get; set; }
        public List<UserSignUpInfo_Identities>? identities { get; set; }
    }

    public class UserSignUpInfo_Attributes
    {
        public UserSignUpInfo_Email? email { get; set; }
        public UserSignUpInfo_City? city { get; set; }
        public UserSignUpInfo_Country? country { get; set; }
        public UserSignUpInfo_DisplayName? displayName { get; set; }
    }

    public class UserSignUpInfo_City
    {
        public string? value { get; set; }

        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }
        public string attributeType { get; set; }
    }

    public class UserSignUpInfo_Country
    {
        public string? value { get; set; }

        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }
        public string attributeType { get; set; }
    }

    public class UserSignUpInfo_DisplayName
    {
        public string? value { get; set; }

        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }
        public string attributeType { get; set; }
    }

    public class UserSignUpInfo_Email
    {
        public string? value { get; set; }

        [JsonPropertyName("@odata.type")]
        public string odatatype { get; set; }
        public string attributeType { get; set; }
    }

    public class UserSignUpInfo_Identities
    {
        public string signInType { get; set; }
        public string issuer { get; set; }
        public string issuerAssignedId { get; set; }
    }
}