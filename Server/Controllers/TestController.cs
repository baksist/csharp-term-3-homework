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
        // GET
        [HttpGet]
        public string Get()
        {
            return "this is a test string";
        }
    }
}