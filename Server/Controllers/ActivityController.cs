using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
    /// <summary>
    /// Controller for monitoring user activity (online/offline)
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ActivityController : ControllerBase
    {
        /// <summary>
        /// GET request for all active users
        /// </summary>
        /// <returns>List of all active users</returns>
        [HttpGet]
        public List<ActiveUser> Get()
        {
            return Program.ActiveUsers;
        }
        
        /// <summary>
        /// POST request which provides info that user is active.
        /// If user is already online, updates its LastSeen property.
        /// Otherwise, adds the user to ActiveUsers list.
        /// </summary>
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