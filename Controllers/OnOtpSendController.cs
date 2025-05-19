using System.Net;
using Azure.Communication.Email;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using woodgroveapi.Helpers;
using woodgroveapi.Models;

namespace woodgroveapi.Controllers;

[Authorize(AuthenticationSchemes = "EntraExternalIdCustomAuthToken")]
[ApiController]
[Route("[controller]")]
public class OnOtpSendController : ControllerBase
{
    private readonly ILogger<OnOtpSendController> _logger;
    private readonly IConfiguration _configuration;
    private TelemetryClient _telemetry;

    public OnOtpSendController(ILogger<OnOtpSendController> logger, TelemetryClient telemetry, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _telemetry = telemetry;
    }

    [HttpPost(Name = "OnOtpSend")]
    public async Task<IActionResult> PostAsync([FromBody] OnOtpSendRequest requestPayload)
    {
        if (requestPayload == null || requestPayload.data == null || requestPayload.data.otpContext == null)
        {
            _logger.LogWarning("Invalid request payload received in OnOtpSend.");
            return BadRequest(new { error = "Request payload, data, or OTP context is null." });
        }

        try
        {
            // Track the page view 
            IDictionary<string, string> moreProperties = new Dictionary<string, string>();
            if (requestPayload.data.otpContext.identifier.IndexOf("@") > 0)
                moreProperties.Add("Identifier", requestPayload.data.otpContext.identifier.Substring(0, 1) + "_" + requestPayload.data.otpContext.identifier.Split("@")[1]);

            AppInsightsHelper.TrackApi("OnOtpSend", this._telemetry, requestPayload.data, moreProperties);

            //For Azure App Service with Easy Auth, validate the azp claim value
            // if (!AzureAppServiceClaimsHeader.Authorize(this.Request))
            // {
            //     AppInsightsHelper.TrackError("OnOtpSend", new Exception("Unauthorized"), this._telemetry, requestPayload.data);
            //     Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //     return null;
            // }

            if (_configuration.GetSection("AppSettings:EmailConnectionString").Value == "")
            {
                return Ok(new OnOtpSendResponse());
            }

            var emailClient = new EmailClient(_configuration.GetSection("AppSettings:EmailConnectionString").Value);

            var subject = "Your Woodgrove account verification code";
            var htmlContent = @$"<html><body>
            <div style='background-color: #1F6402!important; padding: 15px'>
                <table>
                <tbody>
                    <tr>
                        <td colspan='2' style='padding: 0px;font-family: ""Segoe UI Semibold"", ""Segoe UI Bold"", ""Segoe UI"", ""Helvetica Neue Medium"", Arial, sans-serif;font-size: 17px;color: white;'>Woodgrove Groceries live demo</td>
                    </tr>
                    <tr>
                        <td colspan='2' style='padding: 15px 0px 0px;font-family: ""Segoe UI Light"", ""Segoe UI"", ""Helvetica Neue Medium"", Arial, sans-serif;font-size: 35px;color: white;'>Your Woodgrove verification code</td>
                    </tr>
                    <tr>
                        <td colspan='2' style='padding: 25px 0px 0px;font-family: ""Segoe UI"", Tahoma, Verdana, Arial, sans-serif;font-size: 14px;color: white;'> To access <span style='font-family: ""Segoe UI Bold"", ""Segoe UI Semibold"", ""Segoe UI"", ""Helvetica Neue Medium"", Arial, sans-serif; font-size: 14px; font-weight: bold; color: white;'>Woodgrove Groceries</span>'s app, please copy and enter the code below into the sign-up or sign-in page. This code is valid for 30 minutes. </td>
                    </tr>
                    <tr>
                        <td colspan='2' style='padding: 25px 0px 0px;font-family: ""Segoe UI"", Tahoma, Verdana, Arial, sans-serif;font-size: 14px;color: white;'>Your account verification code:</td>
                    </tr>
                    <tr>
                        <td style='padding: 0px;font-family: ""Segoe UI Bold"", ""Segoe UI Semibold"", ""Segoe UI"", ""Helvetica Neue Medium"", Arial, sans-serif;font-size: 25px;font-weight: bold;color: white;padding-top: 5px;'>
                        {requestPayload.data.otpContext.onetimecode}</td>
                        <td rowspan='3' style='text-align: center;'>
                            <img src='https://woodgrovedemo.com/custom-email/shopping.png' style='border-radius: 50%; width: 100px'>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding: 25px 0px 0px;font-family: ""Segoe UI"", Tahoma, Verdana, Arial, sans-serif;font-size: 14px;color: white;'> If you didn't request a code, you can ignore this email. </td>
                    </tr>
                    <tr>
                        <td style='padding: 25px 0px 0px;font-family: ""Segoe UI"", Tahoma, Verdana, Arial, sans-serif;font-size: 14px;color: white;'> Best regards, </td>
                    </tr>
                    <tr>
                        <td>
                            <img src='https://woodgrovedemo.com/Company-branding/headerlogo.png' height='20'>
                        </td>
                        <td style='font-family: ""Segoe UI"", Tahoma, Verdana, Arial, sans-serif;font-size: 14px;color: white; text-align: center;'>
                            <a href='https://woodgrovedemo.com/Privacy' style='color: white; text-decoration: none;'>Privacy Statement</a>
                        </td>
                    </tr>
                </tbody>
                </table>
            </div>
            </body></html>";

            var sender = "donotreply@woodgrovedemo.com";

            EmailSendOperation emailSendOperation = await emailClient.SendAsync(
                Azure.WaitUntil.Started,
                sender,
                requestPayload.data.otpContext.identifier,
                subject,
                htmlContent);

        }
        catch (System.Exception ex)
        {
            AppInsightsHelper.TrackError("OnOtpSend", ex, this._telemetry, requestPayload.data);
        }

        return Ok(new OnOtpSendResponse());
    }
}