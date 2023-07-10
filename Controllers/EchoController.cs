using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using woodgroveapi.Models;

namespace woodgroveapi.Controllers;


//[Authorize]
[ApiController]
[Route("[controller]")]
public class EchoController : ControllerBase
{
    private readonly ILogger<EchoController> _logger;

    public EchoController(ILogger<EchoController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "Echo")]
    public async Task<object> PostAsync()
    {
        _logger.LogInformation($"#### call to: {this.GetType().Name}");

        // Validate that REST API received a bearer token in the authorization header.
        if (Request.Headers.Authorization.Count == 0)
        {
            _logger.LogInformation("#### authorization header not found");
        }
        else
        {
            _logger.LogInformation($"#### authorization header: {Request.Headers.Authorization[0]}");
        }

        // Echo the input data
        string requestBody = await new StreamReader(this.Request.Body).ReadToEndAsync();

        _logger.LogInformation($"#### {requestBody}");

        return "Echo";
    }
}

