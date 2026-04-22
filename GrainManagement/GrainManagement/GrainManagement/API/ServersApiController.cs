using GrainManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.API;

[UseAdminConnection]
[ApiController]
[Route("api/Servers")]
public sealed class ServersApiController : ControllerBase
{
    private readonly dbContext _ctx;
    private readonly ILogger<ServersApiController> _logger;

    public ServersApiController(dbContext ctx, ILogger<ServersApiController> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    // GET /api/Servers
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var data = await _ctx.Servers
            .AsNoTracking()
            .OrderBy(s => s.FriendlyName ?? s.ServerName)
            .Select(s => new
            {
                s.ServerId,
                s.ServerName,
                s.FriendlyName,
                s.IsActive,
                s.Url,
                s.CreatedAt,
                s.UpdatedAt,
            })
            .ToListAsync(ct);

        return Ok(data);
    }

    // POST /api/Servers
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateServerDto dto,
        CancellationToken ct)
    {
        if (dto is null)
            return BadRequest(new { message = "Request body is required." });
        if (dto.ServerId <= 0)
            return BadRequest(new { message = "ServerId is required." });
        if (string.IsNullOrWhiteSpace(dto.ServerName))
            return BadRequest(new { message = "ServerName is required." });

        var exists = await _ctx.Servers.AnyAsync(s => s.ServerId == dto.ServerId, ct);
        if (exists)
            return Conflict(new { message = "A server with this ServerId already exists." });

        var serverName   = dto.ServerName.Trim();
        var friendlyName = dto.FriendlyName?.Trim();
        var url          = dto.Url?.Trim();

        if (await _ctx.Servers.AnyAsync(s => s.ServerName == serverName, ct))
            return Conflict(new { message = $"A server with Server Name '{serverName}' already exists." });

        if (!string.IsNullOrEmpty(friendlyName) &&
            await _ctx.Servers.AnyAsync(s => s.FriendlyName == friendlyName, ct))
            return Conflict(new { message = $"A server with Friendly Name '{friendlyName}' already exists." });

        if (!string.IsNullOrEmpty(url) &&
            await _ctx.Servers.AnyAsync(s => s.Url == url, ct))
            return Conflict(new { message = $"A server with URL '{url}' already exists." });

        var server = new Server
        {
            ServerId     = dto.ServerId,
            ServerName   = serverName,
            FriendlyName = friendlyName,
            IsActive     = dto.IsActive,
            Url          = url,
        };

        _ctx.Servers.Add(server);

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create Server {ServerId}", dto.ServerId);
            return StatusCode(500, new { message = "Database error while creating server." });
        }

        return Ok(new { server.ServerId });
    }

    // PUT /api/Servers/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateServerDto dto,
        CancellationToken ct)
    {
        if (dto is null)
            return BadRequest(new { message = "Request body is required." });

        var server = await _ctx.Servers.FindAsync(new object[] { id }, ct);
        if (server is null)
            return NotFound(new { message = "Server not found." });

        if (dto.ServerName != null)
        {
            var serverName = dto.ServerName.Trim();
            if (await _ctx.Servers.AnyAsync(s => s.ServerId != id && s.ServerName == serverName, ct))
                return Conflict(new { message = $"A server with Server Name '{serverName}' already exists." });
            server.ServerName = serverName;
        }
        if (dto.FriendlyName != null)
        {
            var friendlyName = dto.FriendlyName.Trim();
            if (!string.IsNullOrEmpty(friendlyName) &&
                await _ctx.Servers.AnyAsync(s => s.ServerId != id && s.FriendlyName == friendlyName, ct))
                return Conflict(new { message = $"A server with Friendly Name '{friendlyName}' already exists." });
            server.FriendlyName = friendlyName;
        }
        if (dto.IsActive.HasValue)
            server.IsActive = dto.IsActive.Value;
        if (dto.Url != null)
        {
            var url = dto.Url.Trim();
            if (!string.IsNullOrEmpty(url) &&
                await _ctx.Servers.AnyAsync(s => s.ServerId != id && s.Url == url, ct))
                return Conflict(new { message = $"A server with URL '{url}' already exists." });
            server.Url = url;
        }

        server.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update Server {ServerId}", id);
            return StatusCode(500, new { message = "Database error while updating server." });
        }

        return Ok(new { id });
    }

    // DELETE /api/Servers/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var server = await _ctx.Servers.FindAsync(new object[] { id }, ct);
        if (server is null)
            return NotFound(new { message = "Server not found." });

        _ctx.Servers.Remove(server);

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to delete Server {ServerId}", id);
            return StatusCode(500, new { message = "Database error while deleting server. It may be referenced by other records." });
        }

        return Ok(new { id });
    }
}

public sealed class CreateServerDto
{
    public int ServerId { get; set; }
    public string ServerName { get; set; }
    public string FriendlyName { get; set; }
    public bool IsActive { get; set; } = true;
    public string Url { get; set; }
}

public sealed class UpdateServerDto
{
    public string ServerName { get; set; }
    public string FriendlyName { get; set; }
    public bool? IsActive { get; set; }
    public string Url { get; set; }
}
