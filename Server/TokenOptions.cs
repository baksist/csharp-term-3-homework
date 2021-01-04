using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Server
{
    public class TokenOptions
    {
        public const string ISSUER = "ChatAuthServer";
        public const string AUDIENCE = "ChatUser";
        private const string KEY = "This is a secret key.";
        public const int LIFETIME = 90;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}