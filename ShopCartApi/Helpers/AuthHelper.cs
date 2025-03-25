using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopCartApi.Helpers
{
    public class AuthHelper
    {
        private readonly IConfiguration _configuration;
        public AuthHelper(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public byte[] GetPasswordHash(string Password, byte[] PasswordSalt)
        {

            //2. get the password key from the apppSettings.json file
            string passwordSaltPlusString = _configuration.GetSection("AppSettings:PasswordKey").Value + Convert.ToBase64String(PasswordSalt);

            //3. Create passworrdHash
            // prf => Pseudo Random Functionalityy
            byte[] PasswordHash = KeyDerivation.Pbkdf2(
                password: Password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
               );

            return PasswordHash;
        }
        public string CreateJWToken(int userId)
        {
            //1.Create Claimes
            Claim[] claims = new Claim[]
            {
                new Claim("UserId", userId.ToString())
            };
            // Setting up the signature made up a few distinct parts

            //3. get the password key from the apppSettings.json file
            string? appSettingsTokenKey = _configuration.GetSection("AppSettings:TokenKey")?.Value;


            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(appSettingsTokenKey ?? "")
                );

            SigningCredentials credentials = new SigningCredentials(
                symmetricSecurityKey,
                SecurityAlgorithms.HmacSha256Signature
                );

            //NOTE: securityTokenDescriptor  =  credentials + claimss
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(1),
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            //Create token then store it
            SecurityToken token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(token);



        }
    }
}
