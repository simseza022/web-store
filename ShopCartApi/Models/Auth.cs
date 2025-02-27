using Microsoft.AspNetCore.Identity;

namespace ShopCartApi.Models
{

    public class Auth
    {
        public string Email { get; set; } = "";
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Auth() 
        {
            if(PasswordHash == null)
            {
                PasswordHash = new byte[0];
            }
            if (PasswordSalt == null)
            {
                PasswordSalt = new byte[0];
            }
        }
    }
}
