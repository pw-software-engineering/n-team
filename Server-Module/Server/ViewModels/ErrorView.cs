using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels
{
    public class ErrorView
    {
        public string Error { get; }
        public ErrorView(string error)
        {
            Error = error;
        }
    }
}
