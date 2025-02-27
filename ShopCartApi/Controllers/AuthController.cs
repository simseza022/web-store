using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ShopCartApi.DataAccessLayer;
using ShopCartApi.DataAccessLayer.Repositories;
using ShopCartApi.Dtos;
using ShopCartApi.Models;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace ShopCartApi.Controllers
{
    public class AuthController : Controller
    {
        private readonly DataContextDapper _dataContextDapper;
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _dataContextDapper = new DataContextDapper(configuration);
            _configuration = configuration;
        }
        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDto userForRegistration)
        {
            // check if the passwords match
            if(userForRegistration.Password.Equals(userForRegistration.PasswordConfirmation))
            {
                // 1. Check of the  user already exists
                //var userDb = _userRepository.getUsers().
                string sqlGetUserByEmail = $"SELECT Email FROM ShopCartAppSchema.Auth WHERE Auth.Email = '{userForRegistration.Email}'";
                IEnumerable<string> users = _dataContextDapper.LoadData<string>(sqlGetUserByEmail);
                if (users.Count() == 0) // user doesn't exist
                {
                    // password logic

                    //1. Create password salt
                    // Create a random number generator to get a random number that will be hashed into a password salt byte array

                    byte[] PasswordSalt = new byte[128/8];

                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create()) 
                    {
                        rng.GetNonZeroBytes(PasswordSalt); //sets the random number to our passwordSalt as a byte array
                    }

                    //2. get the password key from the apppSettings.json file
                    string passwordSaltPlusString = _configuration.GetSection("AppSettings:PasswordKey").Value + Convert.ToBase64String(PasswordSalt);

                    //3. Create passworrdHash
                    // prf => Pseudo Random Functionalityy
                    byte[] PasswordHash = KeyDerivation.Pbkdf2(
                        password: userForRegistration.Password,
                        salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 100000,
                        numBytesRequested: 256/8
                       );

                    string sqlAddAuth = "INSERT INTO ShopCartAppSchema.Auth "
                        + "([Email], [PasswordHash], [PasswordSalt]) VALUES "
                        + $"('{userForRegistration.Email}' , @PasswordHash, @PasswordSalt)";

                    //create sql parameters
                    List<SqlParameter> sqlParameters = new List<SqlParameter>();

                    SqlParameter passwordSaltParam = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                    passwordSaltParam.Value = PasswordSalt;
                    SqlParameter passwordHashParam = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
                    passwordSaltParam.Value = PasswordHash;

                    //add them to a list of parameters
                    sqlParameters.Add(passwordSaltParam);
                    sqlParameters.Add(passwordHashParam);

                    if(_dataContextDapper.ExecuteSqlWitParameters(sqlAddAuth, sqlParameters))
                    {
                        return Ok();

                    }

                    throw new Exception("Failed to Register user");


                }
               
                throw new Exception("User with the same email already exists");
                

            }
            throw new Exception("Passwords do not match");
        }

        [HttpPost("Login")]
        public IActionResult Login(UserForLoginConfirmationDto userForLogin)
        {
            return Ok();
        }

    }
}
