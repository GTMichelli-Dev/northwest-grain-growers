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
    public class UsersController : Controller
    {

        private readonly ICurrentUser _me;



        private readonly ILogger<UsersController> _logger;
        private readonly dbContext _context;
        private readonly ITokenAcquisition _tokenAcq;
        private readonly IHttpClientFactory _http;
        public UsersController(ITokenAcquisition tokenAcq, IHttpClientFactory http, dbContext context, ILogger<UsersController> logger, ICurrentUser me)
        {
            _tokenAcq = tokenAcq;
            _http = http;
            _context = context;
            _logger = logger;
            _me = me;
        }




        // GET: UsersController
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
