using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pollr.AdminUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pollr.AdminUI.Controllers
{
    public class ClientConfigurationController: Controller
    {
        ClientConfiguration clientConfig;

        public ClientConfigurationController(IOptions<ClientConfiguration> clientConfigOptions)
        {
            clientConfig = clientConfigOptions.Value;
        }

        [HttpGet]
        [Route("[controller]")]
        public IActionResult Index()
        {
            return Json(clientConfig);
        }
    }
}
