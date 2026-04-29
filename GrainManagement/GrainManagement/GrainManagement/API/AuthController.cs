using GrainManagement.Controllers;
using GrainManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private const int RemoteAdminPrivilegeId = 7;

        private readonly dbContext _ctx;
        private readonly ILogger<AuthController> _logger;

        public AuthController(dbContext ctx, ILogger<AuthController> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public class ValidateRemoteAdminPinDto
        {
            public int Pin { get; set; }
        }

        // POST /api/Auth/ValidateRemoteAdminPin
        // Validates a PIN belongs to an active user that has the Remote Admin
        // privilege (PrivilegeId=7), and on success drops a session cookie that
        // RemoteAdminController checks before rendering the admin tiles.
        [HttpPost("ValidateRemoteAdminPin")]
        public async Task<IActionResult> ValidateRemoteAdminPin(
            [FromBody] ValidateRemoteAdminPinDto dto,
            CancellationToken ct)
        {
            if (dto is null || dto.Pin <= 0)
                return BadRequest(new { message = "PIN is required." });

            var user = await _ctx.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Pin == dto.Pin && u.IsActive, ct);

            if (user is null)
                return Unauthorized(new { message = "Invalid or inactive PIN." });

            var hasRemoteAdmin = await _ctx.UserPrivileges
                .AsNoTracking()
                .AnyAsync(p => p.UserId == user.UserId && p.PrivilegeId == RemoteAdminPrivilegeId, ct);

            if (!hasRemoteAdmin)
                return Unauthorized(new { message = "User does not have the Remote Admin privilege." });

            // Session cookies (no MaxAge / Expires) — cleared when the browser closes.
            // The PIN cookie is the auth marker. The UserName cookie is read by the
            // layout to display "Remote Administrator: <name>" in the navbar.
            var cookieOptions = new CookieOptions
            {
                HttpOnly    = true,
                IsEssential = true,
                SameSite    = SameSiteMode.Lax,
            };
            Response.Cookies.Append(RemoteAdminController.CookieName, user.UserId.ToString(), cookieOptions);
            Response.Cookies.Append(RemoteAdminController.UserNameCookieName,
                Uri.EscapeDataString(user.UserName ?? ""), cookieOptions);

            _logger.LogInformation("Remote Admin PIN accepted for UserId={UserId}", user.UserId);
            return Ok(new { UserId = user.UserId, UserName = user.UserName });
        }
    }
}
