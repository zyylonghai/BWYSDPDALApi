using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BWYSDPDALApi.Controllers
{
    [Route("Srv/[controller]")]
    public class ServerConfigController : Controller
    {
        public IActionResult ServerConfig()
        {
            return View();
        }
    }
}