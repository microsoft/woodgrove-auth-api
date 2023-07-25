# Woodgrove groceries demo of the claims augmentation REST API

This dotnet C# Web API demonstrates how to use Microsoft Entra External ID's custom authentication extension for various events. 

## Endpoints

The sample code provides an implementation of the following endpoints:

### Token issuance start

The *TokenIssuanceStart* event is triggered when a token is about to be issued by Microsoft Entra External ID to your application. When the event is triggered your custom extension REST API is called to fetch attributes from external systems. In this demo, the [TokenIssuanceStartController](./Controllers/TokenIssuanceStartController.cs) returns the following claims:

- **CorrelationId** the correlation ID that was sent by the issuer to your REST API.
- **ApiVersion** a fixed value with your REST API version. This attribute can help you debug your REST API and check if your latest version is in used.
- **LoyaltyNumber** a random numeric value that represents an imaginary loyally number.
- **LoyaltySince** a random date that the that represents an imaginary time the user joined the loyalty program.
- **LoyaltyTier** a random string that the that represents an imaginary loyalty program tier.

The REST API endpoint URL:

```http
POST https://auth-api.woodgrovedemo.com/OnTokenIssuanceStart
```

### On attribute collection start

The *OnAttributeCollectionStart* is fired at the beginning of the attribute collection process and can be used to prevent the user from signing up (such as based on the domain they are authenticating from) or modify the initial attributes to be collected (such as including additional attributes to collect based on the user’s identity provider).

> [!IMPORTANT]
> The OnAttributeCollectionStart event type is not available yet.

### On attribute collection submit

OnAttributeCollectionSubmit event is fired after the user provides attribute information during signing up and can be used to validate the information provided by the user (such as an invitation code or partner number), modify the collected attributes (such as address validation), and either allow the user to continue in the journey or show a validation or block page.

> [!IMPORTANT]
> The OnAttributeCollectionSubmit event type is subject be to changed. Don't use it in your Microsoft Entra External ID tenant. 

This demo validates the city name, against a list of cities and countries we compiled. You can find the list of countries and cities in the [OnAttributeCollectionSubmitController](./Controllers/OnAttributeCollectionSubmitController.cs). 

The REST API endpoint URL:

```http
POST https://auth-api.woodgrovedemo.com/OnAttributeCollectionSubmit
```

## Protect access to your REST API

To ensure the communications between Microsoft Entra custom extension and your REST API are secured appropriately, Microsoft Entra External ID uses OAuth 2.0 client credentials grant flow to issue an access token for the resource application registered with your custom authentication extension. 

When the custom extension calls your REST API, it sends an HTTP Authorization header with a bearer token issued by Azure AD. You REST API validate the access token and its claims values. 

### [Option 1] Validate the access token in your code

This example uses the [Microsoft.Identity.Web](https://www.nuget.org/packages/Microsoft.Identity.Web) library to validate the access token.

In the [appsettings.json](./appsettings.json) file, update the following keys under the `AzureAd` element:

- **ClientId** the application ID that is associated with your custom extension. You can find this application under the API authentication in your custom extension.
- **Audience** same as above
- **TenantId** your tenant ID

This demo REST API can be used without authentication (see option 2 below). If you run your own REST API, uncomment the `[Authorize]` attribute in the controllers. The following example shows how a controller should look like:

```csharp
[Authorize]
[ApiController]
[Route("[controller]")]
public class TokenIssuanceStartController : ControllerBase
{
    // Rest of your code
}
```

### [Option 2] Validate the access token via Azure Service App

[Azure App Service](https://learn.microsoft.com/azure/app-service/) enables you to build and host web apps and and RESTful APIs in the programming language of your choice without managing the infrastructure.

Azure App Service provides built-in [authentication and authorization capabilities](https://learn.microsoft.com/azure/app-service/overview-authentication-authorization) (sometimes referred to as "Easy Auth"), so you can validate the access token sends by Microsoft Entra External ID by writing minimal code in RESTful API.

To enable authentication into your App Service app, follow these steps:

1. Sign in to the [Azure portal](https://portal.azure.com/) and navigate to the app service hosting your web API.
1. From the left navigation, select **Authentication** > **Add identity provider** > **Microsoft**.
1. For **App registration type**, choose **Provide the details of an existing app registration** 
1. Fill in the following configuration details:

    |Field|Description|
    |-|-|
    |Application (client) ID| The application ID that is associated with your custom extension. You can find this application under the API authentication in your custom extension. |
    |Client Secret| Enter any value, such as 12345. |
    |Issuer Url| Use `https://login.microsoftonline.com/<tenant-id>/v2.0`, and replace the *\<tenant-id>* with the **Directory (tenant) ID** in which the app registration was created. |
    |Allowed Token Audiences| Use the same value as the *Application (client) ID*. |
    
1. For the **Restrict access**, select **Require authentication**.
1. For the **Unauthenticated requests**, select **HTTP 401 Unauthorized: recommended for APIs**.
1. Unselect the **Token store** option.
1. Select **Add**.

You're now ready to use the Microsoft identity platform for authentication in your app. The App Service makes the claims in the incoming token available to your code by injecting them into the `X-MS-CLIENT-PRINCIPAL` request header (Base64 encoded JSON representation of available claims). 

To ensure the communications between the custom extension and your REST API are [secured appropriately](https://learn.microsoft.com/azure/active-directory/develop/custom-extension-overview#protect-your-rest-api), validate that the respective `azp` claim equals the `99045fe1-7639-4a75-9d4a-577b6ca3810f` value.

In your REST API use the code in the [AzureAppServiceClaims class](./Models/AzureAppServiceClaims.cs). Then, in the controller call the `Authorize` function that checks the `azp` claim value.

```csharp
if (!AzureAppServiceClaimsHeader.Authorize(this.Request))
{
    Response.StatusCode = (int)HttpStatusCode.Unauthorized;
    return null;
}
```

### [Option 3] Validate the access token via Azure APIM

[Azure API Management](https://learn.microsoft.com/azure/api-management/api-management-key-concepts) offers a scalable, multi-cloud API management platform for securing, publishing, and analyzing APIs. The [validate-azure-ad-token](https://learn.microsoft.com/azure/api-management/validate-azure-ad-token-policy) policy enforces the existence and validity of a JSON web token (JWT) that was provided by the Microsoft Entra External ID.

The following example policy, when added to the `<inbound>` policy section, checks the value of the `audience` and the `azp` claims in an access token obtained from Microsoft Entra External ID that is presented in the `Authorization` header. It returns an error message if the token is not valid. Configure this policy at a policy scope that it protects all custom authentication extensions REST API endpoints.

```xml
<validate-azure-ad-token tenant-id="your-tenant-ID" header-name="Authorization" failed-validation-httpcode="401" failed-validation-error-message="Unauthorized. Access token is missing or invalid.">
  <client-application-ids>
     <application-id>99045fe1-7639-4a75-9d4a-577b6ca3810f</application-id>
  </client-application-ids>
  <audiences>
     <audience>Your application ID</audience>
  </audiences>
</validate-azure-ad-token>
``` 

Use the following values:

- **tenant-id** your Microsoft Entra External ID tenant ID.
- **Audience** the application ID that is associated with your custom extension. You can find this application under the API authentication in your custom extension.


## Data models

The code sample has the following data models:

- TokenIssuanceStart event [request](./Models/TokenIssuanceStartRequest.cs) and [response](./Models/TokenIssuanceStartResponse.cs)
- OnAttributeCollectionSubmit event [request](./Models/OnAttributeCollectionSubmitRequest.cs) and [response](./Models/OnAttributeCollectionSubmitResponse.cs)
