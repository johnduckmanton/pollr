using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pollr.AdminUI.Models;
using System.Reflection;


namespace Pollr.AdminUI.Controllers
{
    public class ClientConfigurationController: Controller
    {
        ClientConfiguration clientConfig;

        public ClientConfigurationController(IOptions<ClientConfiguration> clientConfigOptions)
        {
            clientConfig = clientConfigOptions.Value;
            clientConfig.AppVersion = typeof(ClientConfigurationController).Assembly
                .GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
        }
        [HttpGet]
        [Route("[controller]")]
        public IActionResult Index()
        {
            return Json(clientConfig);
        }
    }
}
