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
        private const int OfficeAdminPrivilegeId = 14;
        private const int AgvantagePrivilegeId   = 15;

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

        public class ValidateOfficeAdminPinDto
        {
            public int Pin { get; set; }
        }

        // POST /api/Auth/ValidateOfficeAdminPin
        // Validates a PIN belongs to an active user that holds PrivilegeId=14
        // (Office Admin). Strict membership check — Remote Admin (priv 7)
        // does NOT bypass this gate; Maintenance is HQ-office-only and
        // remote operators have to be granted priv 14 explicitly to reach
        // it. On success drops session cookies that AdminController checks
        // before serving the Maintenance pages.
        [HttpPost("ValidateOfficeAdminPin")]
        public async Task<IActionResult> ValidateOfficeAdminPin(
            [FromBody] ValidateOfficeAdminPinDto dto,
            CancellationToken ct)
        {
            if (dto is null || dto.Pin <= 0)
                return BadRequest(new { message = "PIN is required." });

            var user = await _ctx.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Pin == dto.Pin && u.IsActive, ct);

            if (user is null)
                return Unauthorized(new { message = "Invalid or inactive PIN." });

            var hasOfficeAdmin = await _ctx.UserPrivileges
                .AsNoTracking()
                .AnyAsync(p => p.UserId == user.UserId && p.PrivilegeId == OfficeAdminPrivilegeId, ct);

            if (!hasOfficeAdmin)
                return Unauthorized(new { message = "User does not have the Office Admin privilege." });

            var cookieOptions = new CookieOptions
            {
                HttpOnly    = true,
                IsEssential = true,
                SameSite    = SameSiteMode.Lax,
            };
            Response.Cookies.Append(AdminController.OfficeAdminCookieName, user.UserId.ToString(), cookieOptions);
            Response.Cookies.Append(AdminController.OfficeAdminUserNameCookieName,
                Uri.EscapeDataString(user.UserName ?? ""), cookieOptions);

            _logger.LogInformation("Office Admin PIN accepted for UserId={UserId}", user.UserId);
            return Ok(new { UserId = user.UserId, UserName = user.UserName });
        }

        public class ValidateAgvantagePinDto
        {
            public int Pin { get; set; }
        }

        // POST /api/Auth/ValidateAgvantagePin
        // Validates a PIN for the Agvantage push pages (priv 15). Strict
        // membership — Remote Admin (priv 7) does NOT bypass. Drops a
        // session cookie that the Agvantage controllers check before
        // rendering their pages.
        [HttpPost("ValidateAgvantagePin")]
        public async Task<IActionResult> ValidateAgvantagePin(
            [FromBody] ValidateAgvantagePinDto dto,
            CancellationToken ct)
        {
            if (dto is null || dto.Pin <= 0)
                return BadRequest(new { message = "PIN is required." });

            var user = await _ctx.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Pin == dto.Pin && u.IsActive, ct);

            if (user is null)
                return Unauthorized(new { message = "Invalid or inactive PIN." });

            var hasAgvantage = await _ctx.UserPrivileges
                .AsNoTracking()
                .AnyAsync(p => p.UserId == user.UserId && p.PrivilegeId == AgvantagePrivilegeId, ct);

            if (!hasAgvantage)
                return Unauthorized(new { message = "User does not have the Agvantage privilege." });

            var cookieOptions = new CookieOptions
            {
                HttpOnly    = true,
                IsEssential = true,
                SameSite    = SameSiteMode.Lax,
            };
            Response.Cookies.Append(AgvantageWarehouseTransferController.AgvantageCookieName,
                user.UserId.ToString(), cookieOptions);
            Response.Cookies.Append(AgvantageWarehouseTransferController.AgvantageUserNameCookieName,
                Uri.EscapeDataString(user.UserName ?? ""), cookieOptions);

            _logger.LogInformation("Agvantage PIN accepted for UserId={UserId}", user.UserId);
            return Ok(new { UserId = user.UserId, UserName = user.UserName });
        }

        public class LoginDto
        {
            public int Pin { get; set; }
            /// <summary>Optional. When set, the PIN's user must hold this
            /// privilege (or RemoteAdmin, which always bypasses) for the
            /// login to succeed.</summary>
            public int? RequirePriv { get; set; }
        }

        // POST /api/Auth/Login
        //
        // Single user-login endpoint. Replaces the per-role
        // ValidateRemoteAdminPin / ValidateOfficeAdminPin /
        // ValidateAgvantagePin endpoints from the UX side: one PIN entry
        // signs the operator in and surfaces every role they hold.
        //
        // On success drops a unified cookie (GrainMgmt_UserId /
        // GrainMgmt_UserName) plus the legacy per-role cookies for any
        // privileges the user holds. The existing controller gates that
        // still check the legacy cookies keep working without changes,
        // and the navbar can read one canonical user name.
        [HttpPost("Login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto dto,
            CancellationToken ct)
        {
            if (dto is null || dto.Pin <= 0)
                return BadRequest(new { message = "PIN is required." });

            var user = await _ctx.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Pin == dto.Pin && u.IsActive, ct);
            if (user is null)
                return Unauthorized(new { message = "Invalid or inactive PIN." });

            var heldPrivs = await _ctx.UserPrivileges
                .AsNoTracking()
                .Where(p => p.UserId == user.UserId)
                .Select(p => p.PrivilegeId)
                .ToListAsync(ct);

            if (dto.RequirePriv.HasValue &&
                !GrainManagement.Constants.Privileges.HasPrivilege(heldPrivs, dto.RequirePriv.Value))
            {
                return Unauthorized(new { message = "Your PIN does not have the privilege required for this page." });
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly    = true,
                IsEssential = true,
                SameSite    = SameSiteMode.Lax,
            };

            // Unified session cookies — read by the navbar and the new
            // single-source AuthGate helper.
            Response.Cookies.Append("GrainMgmt_UserId", user.UserId.ToString(), cookieOptions);
            Response.Cookies.Append("GrainMgmt_UserName",
                Uri.EscapeDataString(user.UserName ?? ""), cookieOptions);

            // Legacy per-role cookies — populated for every privilege the
            // user actually holds so the existing controller gates light
            // up automatically. RemoteAdmin (priv 7) carries the master
            // bypass, so a holder gets every legacy cookie.
            bool hasRemoteAdmin = heldPrivs.Contains(RemoteAdminPrivilegeId);
            bool hasOfficeAdmin = hasRemoteAdmin || heldPrivs.Contains(OfficeAdminPrivilegeId);
            bool hasAgvantage   = hasRemoteAdmin || heldPrivs.Contains(AgvantagePrivilegeId);

            if (hasRemoteAdmin)
            {
                Response.Cookies.Append(RemoteAdminController.CookieName,
                    user.UserId.ToString(), cookieOptions);
                Response.Cookies.Append(RemoteAdminController.UserNameCookieName,
                    Uri.EscapeDataString(user.UserName ?? ""), cookieOptions);
            }
            if (hasOfficeAdmin)
            {
                Response.Cookies.Append(AdminController.OfficeAdminCookieName,
                    user.UserId.ToString(), cookieOptions);
                Response.Cookies.Append(AdminController.OfficeAdminUserNameCookieName,
                    Uri.EscapeDataString(user.UserName ?? ""), cookieOptions);
            }
            if (hasAgvantage)
            {
                Response.Cookies.Append(AgvantageWarehouseTransferController.AgvantageCookieName,
                    user.UserId.ToString(), cookieOptions);
                Response.Cookies.Append(AgvantageWarehouseTransferController.AgvantageUserNameCookieName,
                    Uri.EscapeDataString(user.UserName ?? ""), cookieOptions);
            }

            _logger.LogInformation(
                "Login accepted for UserId={UserId} (RemoteAdmin={RA}, OfficeAdmin={OA}, Agvantage={AG})",
                user.UserId, hasRemoteAdmin, hasOfficeAdmin, hasAgvantage);

            return Ok(new
            {
                UserId   = user.UserId,
                UserName = user.UserName,
                HasRemoteAdmin = hasRemoteAdmin,
                HasOfficeAdmin = hasOfficeAdmin,
                HasAgvantage   = hasAgvantage,
            });
        }
    }
}
