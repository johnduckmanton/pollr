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
        // The URL of the Web Api
        public string ApiUrl { get; set; }
        //The URL of the SignalR hub
        public string HubUrl { get; set; }
        // The URL to go to to vote
        public string VoteUrl { get; set; }

    }
}
