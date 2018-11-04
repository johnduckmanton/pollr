using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pollr.Api.Dal;
using System.Reflection;

namespace Pollr.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly DatabaseContext _context = null;

        public InfoController(IOptions<DatabaseSettings> settings)
        {
            _context = new DatabaseContext(settings);
        }

        /// <summary>
        /// Get the current status
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(200)]
        public ActionResult GetStatus()
        {
            StatusInfo info = new StatusInfo
            {

                // Get Application Version
                AppVersion = typeof(InfoController).Assembly
                .GetCustomAttribute<AssemblyFileVersionAttribute>().Version,

                // Ping the database to check if the connection is OK
                IsDatabaseConnected = _context.Ping()
            };

            return Ok(info);
        }

        private class StatusInfo
        {
            public string AppVersion { get; set; }
            public bool IsDatabaseConnected { get; set; }
        }
    }
}