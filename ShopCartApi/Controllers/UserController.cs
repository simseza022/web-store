using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopCartApi.DataAccessLayer.Repositories;
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

    
        IUserRepository _userRepository;
        IMapper _mapper;
        public UserController(IConfiguration config) 
        {
            _userRepository = new UserRepository(config);
         
            _mapper = new Mapper(
                new MapperConfiguration( cfg => {
                    cfg.CreateMap<UserToAddDto, User>();
                }));


        }

        // GET: api/users
        [HttpGet]
        public IEnumerable<User> Get()
        {

            return _userRepository.getUsers();
        }

        // GET api/Users/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            User? user = _userRepository.getSingleUser(id);
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
            _userRepository.Add(user);

      

            if(_userRepository.saveChanges())
            {
                return Ok();  
            }

            throw new Exception("Failed to add user");

        }


        // POST api/Users/EditUser
        [HttpPost("EditUser")]
        public IActionResult Put(User value)
        {
            User? userDb = _userRepository.getSingleUser(value.UserId);

            if (userDb != null)
            {
                userDb.FirstName = value.FirstName;
                userDb.LastName = value.LastName;
                userDb.Email = value.Email;
                userDb.Gender = value.Gender;
                userDb.Active = value.Active;
               

            }

            if (_userRepository.saveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to edit user");

        }

            


        // DELETE api/users/5
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            User? userDb = _userRepository.getSingleUser(id);

            if (userDb != null) {
                _userRepository.Remove(userDb);
                if (_userRepository.saveChanges())
                {
                    return Ok();
                }
            }

            throw new Exception("User not found");

        }
    }
}
