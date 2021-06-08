using Microsoft.AspNetCore.Mvc;
using Server.RequestModels.Client;
using Server.Services.Client;

namespace Server.Controllers.Client
{
    [ApiController]
    [Route("api-client")]
    public class ClientLoginController : Controller
    {
        private readonly IClientAccountService _service;
        public ClientLoginController(IClientAccountService clientService)
        {
            _service = clientService;
        }

        [HttpPost("client/login")]
        public IActionResult Login([FromBody] ClientCredentials credentials)
        {
            return _service.Login(credentials);
        }
    }
}
