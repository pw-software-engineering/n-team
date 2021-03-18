using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database
{
    public class ServerDBContext:DbContext
    {
        public ServerDBContext(DbContextOptions<ServerDBContext> options) : base(options)
        {
        }
        //FluentAPI    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
