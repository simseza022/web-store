using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopCartApi.DataContext;
using ShopCartApi.Dtos;
using ShopCartApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopCartApi.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {

        DataContextEF _dataContextEF;
        IMapper _mapper;
        public UserController(IConfiguration config) 
        {
            _dataContextEF = new DataContextEF(config);
            _mapper = new Mapper(
                new MapperConfiguration( cfg => {
                    cfg.CreateMap<UserToAddDto, User>();
                }));


        }

        // GET: api/users
        [HttpGet]
        public IEnumerable<User> Get()
        {

            return _dataContextEF.Users == null?[]: _dataContextEF.Users.ToList();
        }

        // GET api/Users/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            User? user = _dataContextEF.Users?.Where(user => user.UserId == id).FirstOrDefault();
            if (user == null) {
                throw new Exception("User not found");
            }
            return user;
        }

        // POST api/Users/AddUser
        [HttpPost("AddUser")]
        public IActionResult Post(UserToAddDto value)
        {
            User user = _mapper.Map<User>(value);

            _dataContextEF.Users?.Add(user);

            if(_dataContextEF.SaveChanges() > 0)
            {
                return Ok();  
            }

            throw new Exception("Failed to add user");

        }


        // POST api/Users/EditUser
        [HttpPost("EditUser")]
        public IActionResult Put(User value)
        {
            User? userDb = _dataContextEF.Users?
                .Where(user => user.UserId == value.UserId)
                .FirstOrDefault();

            if(userDb != null)
            {
                userDb.FirstName = value.FirstName;
                userDb.LastName = value.LastName;
                userDb.Email = value.Email;
                userDb.Gender = value.Gender;
                userDb.Active = value.Active;
               

            }

            if (_dataContextEF.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to edit user");

        }




        //// DELETE api/<UserController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
