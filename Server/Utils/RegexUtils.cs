using System.Text.RegularExpressions;

namespace Server.Utils
{
    /// <summary>
    /// Class for Regex matching custom hostnames and ports.
    /// </summary>
    public class RegexUtils
    {
        const string validIP = @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";
        const string validHostname = @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$";
        const string validPort = @"^([0-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$";

        /// <summary>
        /// Function that checks if provided IP and port are valid
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool MatchParams(string ip, string port)
        {
            return (Regex.IsMatch(ip, validIP) || Regex.IsMatch(ip, validHostname)) && 
                   Regex.IsMatch(port, validPort);
        }
    }
}