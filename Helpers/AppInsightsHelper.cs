
using System.Net;
using System.Security.Cryptography;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using woodgroveapi.Models;


namespace woodgroveapi.Helpers;

public class AppInsightsHelper
{

    public static void TrackApi(string name, TelemetryClient telemetry, AllRequestData requestData, IDictionary<string, string>? moreProperties = null)
    {
        PageViewTelemetry pageView = new PageViewTelemetry(name);
        pageView.Properties.Add("TenantId", requestData.tenantId);
        pageView.Properties.Add("CorrelationId", requestData.authenticationContext.correlationId);
        pageView.Properties.Add("EventListenerId", requestData.authenticationEventListenerId);
        pageView.Properties.Add("AuthenticationExtensionId", requestData.customAuthenticationExtensionId);
        pageView.Properties.Add("Protocol", requestData.authenticationContext.protocol);
        pageView.Properties.Add("AppDisplayName", requestData.authenticationContext.clientServicePrincipal.appDisplayName);
        pageView.Properties.Add("AppId", requestData.authenticationContext.clientServicePrincipal.appId);

        // OnAttributeCollectionStart's specific properties
        if (requestData is AttributeCollectionRequest_Data)
        {
            AttributeCollectionRequest_Data attributeCollectionRequestData = (AttributeCollectionRequest_Data)requestData;

            if (attributeCollectionRequestData.userSignUpInfo.identities != null && 
            attributeCollectionRequestData.userSignUpInfo.identities.Count > 0)
            {
                pageView.Properties.Add("UserSignUpInfoIssuer", attributeCollectionRequestData.userSignUpInfo.identities[0].issuer);
                pageView.Properties.Add("UserSignUpInfoSignInType", attributeCollectionRequestData.userSignUpInfo.identities[0].signInType);
            
                // For local account get the email suffix
                if (attributeCollectionRequestData.userSignUpInfo.identities[0].signInType == "emailAddress")
                {
                    string email = attributeCollectionRequestData.userSignUpInfo.identities[0].issuerAssignedId;
                    pageView.Properties.Add("UserSignUpInfoEmailSuffix", email.Substring(email.IndexOf('@')));
                }
            }
        }

        if (moreProperties != null)
            foreach (var item in moreProperties)
            {
                pageView.Properties.Add(item.Key, item.Value);
            }

        telemetry.TrackPageView(pageView);
    }

    public static void TrackError(string name, Exception ex, TelemetryClient telemetry, AllRequestData requestData, IDictionary<string, string>? moreProperties = null)
    {
        ExceptionTelemetry exception = new ExceptionTelemetry(ex);
        exception.Properties.Add("ApiName", name);
        exception.Properties.Add("TenantId", requestData.tenantId);
        exception.Properties.Add("CorrelationId", requestData.authenticationContext.correlationId);
        exception.Properties.Add("EventListenerId", requestData.authenticationEventListenerId);
        exception.Properties.Add("AuthenticationExtensionId", requestData.customAuthenticationExtensionId);
        exception.Properties.Add("Protocol", requestData.authenticationContext.protocol);
        exception.Properties.Add("AppDisplayName", requestData.authenticationContext.clientServicePrincipal.appDisplayName);
        exception.Properties.Add("AppId", requestData.authenticationContext.clientServicePrincipal.appId);

        if (moreProperties != null)
            foreach (var item in moreProperties)
            {
                exception.Properties.Add(item.Key, item.Value);
            }

        telemetry.TrackException(exception);
    }
}