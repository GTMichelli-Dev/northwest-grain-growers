using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Seed25.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Seed25.Controllers {

    [Route("api/[controller]")]
    public class SampleDataController : Controller {

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions) {
           
           return DataSourceLoader.Load(Seed25.Models.SampleData.Orders, loadOptions);
        }

    }
}