using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace woodgroveapi.Models
{
    public class AuthenticationContext
    {
        public string correlationId { get; set; }
        public AuthenticationContext_Client client { get; set; }
        public string protocol { get; set; }
        public AuthenticationContext_ServicePrincipal clientServicePrincipal { get; set; }
        public AuthenticationContext_ServicePrincipal resourceServicePrincipal { get; set; }
        public AuthenticationContext_User? user { get; set; }
    }

    public class AuthenticationContext_Client
    {
        public string ip { get; set; }
        public string locale { get; set; }
        public string market { get; set; }
    }

    public class AuthenticationContext_ServicePrincipal
    {
        public string id { get; set; }
        public string appId { get; set; }
        public string appDisplayName { get; set; }
        public string displayName { get; set; }
    }

    public class AuthenticationContext_User
    {
        // Display name
        [StringLength(120, ErrorMessage = "DisplayName length can't be more than 120.")]
        public string? displayName { get; set; }

        // User object ID
        [StringLength(120, ErrorMessage = "ID length can't be more than 120.")]
        public string? id { get; set; }

        // UPN
        [StringLength(120, ErrorMessage = "Surname length can't be more than 120.")]
        public string? userPrincipalName { get; set; }

        // User type
        [StringLength(120, ErrorMessage = "UserType length can't be more than 120.")]
        public string? userType { get; set; }

        // Mail address
        [StringLength(120, ErrorMessage = "Mail length can't be more than 120.")]
        public string? mail { get; set; }
    }
}