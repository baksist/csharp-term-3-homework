using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : Controller
    {
        // GET
        [HttpGet]
        public string Get()
        {
            return "test string";
        }
    }
}