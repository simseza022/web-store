using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using ShopCartApi.DataAccessLayer;
using ShopCartApi.DataAccessLayer.Repositories;
using ShopCartApi.Dtos;
using ShopCartApi.Helpers;
using ShopCartApi.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShopCartApi.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
   
    public class AuthController : Controller
    {
        private readonly DataContextDapper _dataContextDapper;
        private readonly AuthHelper _authHelper;
        public AuthController(IConfiguration configuration)
        {
            _dataContextDapper = new DataContextDapper(configuration);
            _authHelper = new AuthHelper(configuration);
        }


        [AllowAnonymous]
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
                    //3. Create passworrdHash
                    // prf => Pseudo Random Functionalityy
                    byte[] PasswordHash = _authHelper.GetPasswordHash(userForRegistration.Password, PasswordSalt);

                    string sqlAddAuth = "INSERT INTO ShopCartAppSchema.Auth "
                        + "([Email], [PasswordHash], [PasswordSalt]) VALUES "
                        + $"('{userForRegistration.Email}' , @PasswordHash, @PasswordSalt)";

                    //create sql parameters
                    List<SqlParameter> sqlParameters = new List<SqlParameter>();

                    SqlParameter passwordSaltParam = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                    passwordSaltParam.Value = PasswordSalt;
                    SqlParameter passwordHashParam = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
                    passwordHashParam.Value = PasswordHash;

                    //add them to a list of parameters
                    sqlParameters.Add(passwordSaltParam);
                    sqlParameters.Add(passwordHashParam);

                    if(_dataContextDapper.ExecuteSqlWitParameters(sqlAddAuth, sqlParameters))
                    {
                        //add the user to the users table
                        User userToAdd = new User(userForRegistration);
                        string sqlAddUser = @"INSERT INTO ShopCartAppSchema.Users(
                        [FirstName],
                        [LastName],
                        [Email],
                        [Gender],
                        [Active]) VALUES (" +
                        "'"+userForRegistration.FirstName+"', '"+
                            userForRegistration.LastName+ "', '"+ 
                            userForRegistration.Email+ "', '"+
                            userForRegistration.Gender+"', 1)";

                        Console.WriteLine(userToAdd);
                        if(!_dataContextDapper.ExecuteSql(sqlAddUser))
                        {
                            return StatusCode(400, "Error adding user");
                        }
                        return Ok();

                    }

                    throw new Exception("Failed to Register user");


                }
               
                throw new Exception("User with the same email already exists");
                

            }
            throw new Exception("Passwords do not match");
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
            string sqlForHashAndSalt = @"SELECT 
                [PasswordHash],
                [PasswordSalt]
                FROM ShopCartAppSchema.Auth WHERE Auth.Email = '" + userForLogin.Email + "'";
            UserForLoginConfirmationDto userForConfimation  = _dataContextDapper.LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt);

            byte[] PasswordHash = _authHelper.GetPasswordHash(userForLogin.Password, userForConfimation.PasswordSalt);

            //if(PasswordHash == userForConfimation.PasswordHash) // won't work!

            for (int i = 0; i < PasswordHash.Length; i++)
            {
                if (PasswordHash[i] != userForConfimation.PasswordHash[i])
                {
                    return StatusCode(401, "Incorrect password");
                }
                
            }
            string getUserIdSql = @"SELECT UserId FROM ShopCartAppSchema.Users Where Email = '" + userForLogin.Email + "'";
            int userId = _dataContextDapper.LoadDataSingle<int>(getUserIdSql);
            return Ok(new Dictionary<string, string>
            {
                { 
                    "token", _authHelper.CreateJWToken(userId) 
                }
            });
        }

        [HttpGet]
        [Route("RefreshToken")]
        public IActionResult RefreshToken()
        {
            // We get the userId from the the ControllerBase from the Claims
            string userID = User.FindFirst("UserId")?.Value + "";
            // check if the user exists in the DB
            string sqlGetUserId = "SELECT UserId from ShopCartAppSchema.Users Where UserId = " + userID;

            int userIdDb = _dataContextDapper.LoadDataSingle<int>(sqlGetUserId);

            return Ok(new Dictionary<string, string> { { "token", _authHelper.CreateJWToken(userIdDb) } });
        }



    }
}
