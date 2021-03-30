using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Client_Module.MockupApiControllers
{
    [Route("api/client")]
    [ApiController]
    public class ClientApiController : ControllerBase
    {
        public ClientApiController() { }

        [HttpPost("login")]
        public IActionResult LogIn()
        {
            if(!Request.Form.ContainsKey("login"))
            {
                return BadRequest(new { error = "Request body must contain a login property" });
            }
            if(!Request.Form.ContainsKey("password"))
            {
                return BadRequest(new { error = "Request body must contain a password property" });
            }
            if(Request.Form["login"] != "TestUser" || Request.Form["password"] != "password123")
            {
                return Unauthorized(new { error = "Provided credentials are incorrect" });
            }
            ClientToken token = new ClientToken()
            {
                ID = -1,
                CreatedAt = DateTime.Now
            };
            return new JsonResult(token);
        }
        
        [HttpGet("")]
        public IActionResult ClientInfo()
        {
            if(!Request.Headers.ContainsKey("x-client-token"))
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return new JsonResult(new { error = "No client token header in HTTP request" });
            }
            ClientToken token;
            try
            {
                token = JsonSerializer.Deserialize<ClientToken>(Request.Headers["x-client-token"]);
            }
            catch(Exception e)
            {
                return Unauthorized(new { error = $"Provided authentication token is malformed\n{e.Message}" });
            }
            if(token.ID != -1)
            {
                return Unauthorized(new { error = "Invalid authentication token (illegal value)" });
            }
            
            ClientInfo client = new ClientInfo()
            {
                Name = "Jan",
                Surname = "Kowalski",
                Username = "j.kowalski99",
                Email = "jan.kowalski@mailing.net"
            };
            return new JsonResult(client);
        }

        [HttpPatch("")]
        public IActionResult ClientInfo(EditClientInfo editInfo)
        {
            Regex emailRegex = new Regex("^[a-zA-Z]([a-zA-Z]|[\\.-]|[0-9])+@[a-z]{1,20}\\.[a-z]{1,10}$");
            Regex usernameRegex = new Regex("^[a-zA-Z][a-zA-Z0-9]{1, 50}$");
            if(editInfo.Username == "UsernameAlreadyTaken")
            {
                return BadRequest(new { error = "This username is already in use." });
            }
            return Ok();
        }
    }

    public class ClientToken
    {
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ClientInfo
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class EditClientInfo
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
