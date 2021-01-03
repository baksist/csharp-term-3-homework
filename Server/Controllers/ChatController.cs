using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
    /// <summary>
    /// Controller for managing messages in chat.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        /// <summary>
        /// GET request which provides all messages.
        /// </summary>
        /// <returns>List of all messages</returns>
        [HttpGet]
        public List<Message> Get()
        {
            return Program.Messages;
        }

        /// <summary>
        /// POST request that appends a new message to message list.
        /// </summary>
        /// <param name="obj">A new message</param>
        [Authorize]
        [HttpPost]
        public void Post([FromBody] Message obj)
        {
            Program.Messages.Add(obj);
        }
        
        /// <summary>
        /// DELETE requests that deletes the message by its ID number.
        /// Only admin users can delete messages.
        /// </summary>
        /// <param name="id"></param>
        [Authorize(Roles = "admin")]
        [HttpDelete]
        public void Delete(int id)
        {
            Program.Messages.RemoveAt(id);
        }
    }
}