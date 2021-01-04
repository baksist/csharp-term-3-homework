using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;


namespace WinFormsClient
{
    class Authentication
    {
        public static void Authenticate(string newUsername, string newPassword)
        {
            
            var requestData = new
            {
                username = newUsername,
                password = newPassword
            };
            var response = Program.Client.PostAsJsonAsync(Program.AppPath + "/users/token", requestData);
            var rep = response.Result.Content.ReadAsStringAsync().Result;
            Program.Token = JsonConvert.DeserializeObject<Dictionary<string, string>>(rep)["access_token"];
            Program.Role = JsonConvert.DeserializeObject<Dictionary<string, string>>(rep)["role"];
        }
    }
}
