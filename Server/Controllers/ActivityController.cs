using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActivityController : ControllerBase
    {
        [HttpGet]
        public List<ActiveUser> Get()
        {
            return Program.ActiveUsers;
        }
        
        [Authorize]
        [HttpPost]
        public void Post()
        {
            var username = User.Identity.Name;
            var i = Program.ActiveUsers.FindIndex(x => x.Username == username);
            if (i >= 0)
            {
                Program.ActiveUsers[i].LastSeen = DateTime.Now;
            }
            else
            {
                Program.ActiveUsers.Add(new ActiveUser {Username = username, LastSeen = DateTime.Now});
                Program.Messages.Add(new Message($"{username} connected", "system", DateTime.Now));
            }
        }
    }
}