using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BasicWeigh.Web.Data;
using BasicWeigh.Web.Models;
using BasicWeigh.Web.Services;
using System.Security.Claims;

namespace BasicWeigh.Web.Controllers;

public class AccountController : Controller
{
    private readonly ScaleDbContext _db;
    private readonly AppSetupCache _setupCache;

    public AccountController(ScaleDbContext db, AppSetupCache setupCache)
    {
        _db = db;
        _setupCache = setupCache;
    }

    // GET: /Account/Login
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        // If login is not required, redirect home
        var setup = _setupCache.Get();
        if (!setup.UseLogin)
            return RedirectToAction("Index", "Home");

        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
    {
        var setup = _setupCache.Get();
        if (!setup.UseLogin)
            return RedirectToAction("Index", "Home");

        var user = _db.Users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower() && u.Active);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            ViewBag.Error = "Invalid username or password.";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // Check if must change password
        if (user.MustChangePassword)
        {
            TempData["ChangePasswordUserId"] = user.Id;
            return RedirectToAction("ChangePassword");
        }

        await SignInUser(user);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    // GET: /Account/ChangePassword
    [AllowAnonymous]
    public IActionResult ChangePassword()
    {
        if (TempData["ChangePasswordUserId"] == null && !User.Identity!.IsAuthenticated)
            return RedirectToAction("Login");

        return View();
    }

    // POST: /Account/ChangePassword
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(string newPassword, string confirmPassword)
    {
        int userId;
        if (TempData["ChangePasswordUserId"] != null)
        {
            userId = (int)TempData["ChangePasswordUserId"]!;
        }
        else if (User.Identity!.IsAuthenticated)
        {
            userId = int.Parse(User.FindFirst("UserId")!.Value);
        }
        else
        {
            return RedirectToAction("Login");
        }

        if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 4)
        {
            ViewBag.Error = "Password must be at least 4 characters.";
            TempData["ChangePasswordUserId"] = userId;
            return View();
        }

        if (newPassword != confirmPassword)
        {
            ViewBag.Error = "Passwords do not match.";
            TempData["ChangePasswordUserId"] = userId;
            return View();
        }

        var user = _db.Users.Find(userId);
        if (user == null) return RedirectToAction("Login");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.MustChangePassword = false;
        _db.SaveChanges();

        await SignInUser(user);
        TempData["Message"] = "Password changed successfully.";
        return RedirectToAction("Index", "Home");
    }

    // POST: /Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    // ---- User Management (Admin only) ----

    // GET: /Account/Users
    [Authorize(Roles = "Admin")]
    public IActionResult Users()
    {
        var users = _db.Users.Where(u => u.Username != "support").OrderBy(u => u.Username).ToList();
        ViewBag.AdminCount = _db.Users.Count(u => u.Role == "Admin" && u.Active && u.Username != "support");
        return View(users);
    }

    // GET: /Account/CreateUser
    [Authorize(Roles = "Admin")]
    public IActionResult CreateUser()
    {
        return View(new AppUser { Role = "User", MustChangePassword = true });
    }

    // POST: /Account/CreateUser
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public IActionResult CreateUser(AppUser model, string password)
    {
        if (string.IsNullOrWhiteSpace(model.Username))
        {
            ViewBag.Error = "Username is required.";
            return View(model);
        }

        if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
        {
            ViewBag.Error = "Password must be at least 4 characters.";
            return View(model);
        }

        if (model.Username.ToLower() == "support")
        {
            ViewBag.Error = "That username is reserved.";
            return View(model);
        }

        if (_db.Users.Any(u => u.Username.ToLower() == model.Username.ToLower()))
        {
            ViewBag.Error = "Username already exists.";
            return View(model);
        }

        model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        _db.Users.Add(model);
        _db.SaveChanges();

        TempData["Message"] = $"User '{model.Username}' created.";
        return RedirectToAction("Users");
    }

    // GET: /Account/EditUser/5
    [Authorize(Roles = "Admin")]
    public IActionResult EditUser(int id)
    {
        var user = _db.Users.Find(id);
        if (user == null || user.Username == "support") return NotFound();
        ViewBag.AdminCount = _db.Users.Count(u => u.Role == "Admin" && u.Active && u.Username != "support");
        return View(user);
    }

    // POST: /Account/EditUser/5
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public IActionResult EditUser(int id, AppUser model)
    {
        var user = _db.Users.Find(id);
        if (user == null || user.Username == "support") return NotFound();

        // Check for duplicate username
        if (_db.Users.Any(u => u.Username.ToLower() == model.Username.ToLower() && u.Id != id))
        {
            ViewBag.Error = "Username already exists.";
            ViewBag.AdminCount = _db.Users.Count(u => u.Role == "Admin" && u.Active && u.Username != "support");
            return View(user);
        }

        // Prevent changing the last admin's role or deactivating them
        var adminCount = _db.Users.Count(u => u.Role == "Admin" && u.Active && u.Username != "support");
        if (user.Role == "Admin" && user.Active && adminCount <= 1)
        {
            if (model.Role != "Admin" || !model.Active)
            {
                ViewBag.Error = "Cannot change the role or deactivate the last admin user.";
                ViewBag.AdminCount = adminCount;
                return View(user);
            }
        }

        user.Username = model.Username;
        user.DisplayName = model.DisplayName;
        user.Role = model.Role;
        user.Active = model.Active;
        _db.SaveChanges();

        TempData["Message"] = $"User '{user.Username}' updated.";
        return RedirectToAction("Users");
    }

    // POST: /Account/ResetPassword/5
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public IActionResult ResetPassword(int id)
    {
        var user = _db.Users.Find(id);
        if (user == null || user.Username == "support") return NotFound();

        // Reset to default password
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("michelli");
        user.MustChangePassword = true;
        _db.SaveChanges();

        TempData["Message"] = $"Password for '{user.Username}' has been reset. They will be prompted to change it on next login.";
        return RedirectToAction("Users");
    }

    // POST: /Account/DeleteUser/5
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteUser(int id)
    {
        var user = _db.Users.Find(id);
        if (user == null || user.Username == "support") return NotFound();

        // Prevent deleting the last admin
        if (user.Role == "Admin" && _db.Users.Count(u => u.Role == "Admin" && u.Active && u.Username != "support") <= 1)
        {
            TempData["Error"] = "Cannot delete the last admin user.";
            return RedirectToAction("Users");
        }

        _db.Users.Remove(user);
        _db.SaveChanges();

        TempData["Message"] = $"User '{user.Username}' deleted.";
        return RedirectToAction("Users");
    }

    private async Task SignInUser(AppUser user)
    {
        var claims = new List<Claim>
        {
            new("UserId", user.Id.ToString()),
            new(ClaimTypes.Name, user.DisplayName),
            new("Username", user.Username),
            new(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(12)
            });
    }
}
