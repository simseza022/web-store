﻿namespace ShopCartApi.Dtos
{
    public partial class UserForRegistrationDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string PasswordConfirmation { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Gender { get; set; } = "";
        public bool Active { get; set; }

        public UserForRegistrationDto()
        {

        }
        //public UserForRegistrationDto():base()
        //{

        //}
    }
}
