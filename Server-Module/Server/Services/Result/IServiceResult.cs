using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Server.Services.Result
{
    public interface IServiceResult : IActionResult
    {
        HttpStatusCode StatusCode { get; }
        object Result { get; }
    }
}
