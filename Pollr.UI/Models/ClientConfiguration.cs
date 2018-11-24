using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pollr.AdminUI.Models
{
    public class ClientConfiguration
    {
        // Production Mode Flag
        public string Production { get; set; }  
        // App version number
        public string AppVersion { get; set; }
        // The URL of the Web Api
        public string ApiUrl { get; set; }
        //The URL of the SignalR hub
        public string HubUrl { get; set; }

    }
}
