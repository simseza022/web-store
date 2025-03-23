using ShopCartApi.Dtos;

namespace ShopCartApi.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Gender { get; set; } = "";
        public bool Active { get; set; }

        public User()
        {

        }
        public User(UserForRegistrationDto userForReg) : base()
        {
            FirstName = userForReg.FirstName;
            LastName = userForReg.LastName;
            Email = userForReg.Email;
            Gender = userForReg.Gender;
            Active = userForReg.Active;
        }

    }
}
