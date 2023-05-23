using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace woodgroveapi.Models
{

    public class AuthenticationContext
    {
        public string correlationId { get; set; }
        public Client client { get; set; }
        public string protocol { get; set; }
        public ClientServicePrincipal clientServicePrincipal { get; set; }
        public ResourceServicePrincipal resourceServicePrincipal { get; set; }
        public User? user { get; set; }
    }

    public class Client
    {
        public string ip { get; set; }
        public string locale { get; set; }
        public string market { get; set; }
    }

    public class ClientServicePrincipal
    {
        public string id { get; set; }
        public string appId { get; set; }
        public string appDisplayName { get; set; }
        public string displayName { get; set; }
    }


    public class ResourceServicePrincipal
    {
        public string id { get; set; }
        public string appId { get; set; }
        public string appDisplayName { get; set; }
        public string displayName { get; set; }
    }

    public class User
    {
        // Display name
        [StringLength(120, ErrorMessage = "DisplayName length can't be more than 120.")]
        public string? displayName { get; set; }

        // Give name
        [StringLength(120, ErrorMessage = "GivenName length can't be more than 120.")]
        public string? givenName { get; set; }

        // User object ID
        [StringLength(120, ErrorMessage = "ID length can't be more than 120.")]
        public string? id { get; set; }

        // Mail address
        [StringLength(120, ErrorMessage = "Mail length can't be more than 120.")]
        public string? mail { get; set; }

        // User peferred language
        [StringLength(120, ErrorMessage = "PeferredLanguage length can't be more than 120.")]
        public string? preferredLanguage { get; set; }

        // Surname
        [StringLength(120, ErrorMessage = "Surname length can't be more than 120.")]
        public string? surname { get; set; }

        // UPN
        [StringLength(120, ErrorMessage = "Surname length can't be more than 120.")]
        public string? userPrincipalName { get; set; }

        // User type
        [StringLength(120, ErrorMessage = "UserType length can't be more than 120.")]
        public string? userType { get; set; }

        // Country
        [StringLength(120, ErrorMessage = "Country length can't be more than 120.")]
        public string? country { get; set; }

        // City
        [StringLength(120, ErrorMessage = "City length can't be more than 120.")]
        public string? city { get; set; }
    }
}