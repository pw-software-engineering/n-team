using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
    static class ControllerExtension
    {
        public static async Task<IActionResult> TrySendAsync(this Controller controller, Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (HttpRequestException e)
            {
                return controller.StatusCode((int)(e.StatusCode ?? HttpStatusCode.InternalServerError));
            }
            catch (Exception)
            {
                return controller.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
