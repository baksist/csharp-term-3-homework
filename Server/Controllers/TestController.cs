using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public List<Message> Get()
        {
            return Program.Messages;
        }

        [HttpPost]
        public void Post([FromBody] Message obj)
        {
            Program.Messages.Add(obj);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            Program.Messages.RemoveAt(id);
        }
    }
}