using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Exceptions
{
    public class NotOwnerException:Exception
    {
        public NotOwnerException() : base("Not owner of this resource") { }
        public NotOwnerException(string message) : base(message) { }
    }
}
