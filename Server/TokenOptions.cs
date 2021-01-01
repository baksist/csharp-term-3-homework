using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Server
{
    public class TokenOptions
    {
        public const string ISSUER = "ChatAuthServer";
        public const string AUDIENCE = "ChatUser";
        private const string KEY = "AHDHWAkdshdhasDUKHAWD";
        public const int LIFETIME = 1;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}