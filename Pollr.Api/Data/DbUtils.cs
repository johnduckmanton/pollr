using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Pollr.Api.Data
{
    public class DbUtils
    {
        private readonly PollrContext _context = null;

        public DbUtils(DbContextOptions<PollrContext> options)
        {
            _context = new PollrContext(options);
        }

        public bool Ping()
        {
            return true;
        }
    }
}
