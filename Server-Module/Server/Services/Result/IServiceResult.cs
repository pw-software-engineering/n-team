using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Server.Services.Result
{
    public interface IServiceResult : IActionResult
    {
        HttpStatusCode StatusCode { get; }
        object Result { get; }
    }
}
