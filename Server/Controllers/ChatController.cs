using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        [HttpGet]
        public List<Message> Get()
        {
            return Program.Messages;
        }

        [Authorize]
        [HttpPost]
        public void Post([FromBody] Message obj)
        {
            Program.Messages.Add(obj);
        }
        
        [Authorize(Roles = "admin")]
        [HttpDelete]
        public void Delete(int id)
        {
            Program.Messages.RemoveAt(id);
        }
    }
}