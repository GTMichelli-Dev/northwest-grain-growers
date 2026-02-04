using GrainManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace GrainManagement
{
    [Authorize]
    [AuthorizeForScopes(Scopes = new[] { "Group.Read.All" })]
    public class AccountsController : Controller
    {

        private readonly ICurrentUser _me;



        private readonly ILogger<AccountsController> _logger;
        private readonly dbContext _context;
        private readonly ITokenAcquisition _tokenAcq;
        private readonly IHttpClientFactory _http;
        public AccountsController(ITokenAcquisition tokenAcq, IHttpClientFactory http, dbContext context, ILogger<AccountsController> logger, ICurrentUser me)
        {
            _tokenAcq = tokenAcq;
            _http = http;
            _context = context;
            _logger = logger;
            _me = me;
        }

        public IActionResult Index()
        {
            if (!_me.IsManager && !_me.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
