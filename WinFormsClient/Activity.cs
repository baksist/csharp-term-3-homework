using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace WinFormsClient
{
    class Activity
    {
        private const int ActivityPeriod = 150;
        public static void Update()
        {
            while (true)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, Program.AppPath + "/activity");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Program.Token);
                var response = Program.Client.SendAsync(request);
                Thread.Sleep(ActivityPeriod);
            }
        }
    }
}
