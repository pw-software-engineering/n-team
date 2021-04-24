using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Server.Services.Response
{
    public class ServiceResult : IServiceResult
    {
        public HttpStatusCode StatusCode { get; }
        public object ResponseBody { get; }

        public ServiceResult(HttpStatusCode code, object body)
        {
            StatusCode = code;
            ResponseBody = body;
        }
        public ServiceResult(HttpStatusCode code)
        {
            StatusCode = code;
        }
    }
}
