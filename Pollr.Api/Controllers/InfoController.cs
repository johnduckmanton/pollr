using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pollr.Api.Data;
using System.Reflection;

namespace Pollr.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly PollrContext _context = null;

        public InfoController(DbContextOptions<PollrContext> options)
        {
            _context = new PollrContext(options);
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