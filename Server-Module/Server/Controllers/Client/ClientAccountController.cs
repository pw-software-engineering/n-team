using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Authentication.Client;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Services.Client;
using Server.RequestModels.Client;

namespace Server.Controllers.Client
{
    [Authorize(AuthenticationSchemes = ClientTokenDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("/api-client")]
    public class ClientAccountController : Controller
    {
        private readonly IClientAccountService _service;
        private int _clientID;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _clientID = int.Parse(HttpContext.User.Claims.First(c => c.Type == ClientTokenManagerOptions.ClientIdClaimName).Value);
            base.OnActionExecuting(context);
        }
        public ClientAccountController(IClientAccountService service)
        {
            _service = service;    
        }
        
        [HttpGet("client")]
        public IActionResult GetClientInfo()
        {
            return _service.GetClientInfo(_clientID);
        }
        
        [HttpPatch("client")]
        public IActionResult PatchClientInfo([FromBody] ClientInfoUpdate editClientInfo)
        {
            return _service.UpdateClientInfo(_clientID, editClientInfo);
        }
    }
}
