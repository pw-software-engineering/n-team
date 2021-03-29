using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Exceptions
{
    public class NotFoundException:Exception
    {
        public NotFoundException() : base("No resource was found") { }
        public NotFoundException(string message) : base(message) { }
    }
}
