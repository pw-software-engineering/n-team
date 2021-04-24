using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Result
{
    public class Error
    {
        string error { get; }
        public Error(string error)
        {
            this.error = error;
        }
    }
}
