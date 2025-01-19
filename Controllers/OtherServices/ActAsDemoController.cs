using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using woodgroveapi.Models;

namespace woodgroveapi.Controllers;


[Authorize(AuthenticationSchemes = "EntraExternalIdUserToken")]
[ApiController]
[Route("[controller]")]
public class ActAsDemoController : ControllerBase
{
    private readonly ILogger<ActAsDemoController> _logger;
    private TelemetryClient _telemetry;
    private readonly IMemoryCache _memoryCache;

    public ActAsDemoController(ILogger<ActAsDemoController> logger, IMemoryCache memoryCache, TelemetryClient telemetry)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _telemetry = telemetry;
    }

    [HttpPost(Name = "ActAs")]
    public  IActionResult  OnPost([FromBody] ActAsRequest request)
    {
        // Check the user object ID
        string? oid = User.Claims.FirstOrDefault(c => c.Type.ToLower() == "oid")?.Value;
        if (oid == null)
        {
            return Unauthorized();
        }

        // Create cache object
        ActAsEntity actAsEntity = new ActAsEntity()
        {
            UserId = oid,
            ActAs = request.ActAs
        };

        var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));
        _memoryCache.Set(oid, actAsEntity, cacheEntryOptions);

        return Ok();
    }
}
