using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;

namespace WinFormsClient
{
    static class Program
    {
        public static string AppPath = "http://localhost:5000";
        public static string Token;
        public static readonly HttpClient Client = new HttpClient();
        public static string Username;
        public static string Password;
        public static string Role;
        public static int[] Size = new int[2];

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GreeterForm());
        }
    }
}
