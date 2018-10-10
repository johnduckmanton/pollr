using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pollr.Api.Models
{
    public class VoteResult
    {
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public string Message { get; set; }
    }
}
