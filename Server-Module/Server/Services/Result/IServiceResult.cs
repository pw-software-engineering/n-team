using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Server.Services.Response
{
    public interface IServiceResult
    {
        HttpStatusCode StatusCode { get; }
        object ResponseBody { get; }
    }
}
