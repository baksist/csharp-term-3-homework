﻿using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace ConsoleClient
{
    public class Activity
    {
        public static void Update()
        {
            while (true)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, Program.appPath + "/activity");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Program.token);
                var response = Program.client.SendAsync(request);
                Thread.Sleep(150);
            }
        }
    }
}