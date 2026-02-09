using GrainManagement.API;
using GrainManagement.Models;
using GrainManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace GrainManagement.Controllers
{
    public class LocationsController : Controller
    {
        private readonly ICurrentUser _me;



        private readonly ILogger<LocationsController> _logger;
        private readonly dbContext _context;
        private readonly ITokenAcquisition _tokenAcq;
        private readonly IHttpClientFactory _http;
        public LocationsController(ITokenAcquisition tokenAcq, IHttpClientFactory http, dbContext context, ILogger<LocationsController> logger, ICurrentUser me)
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
