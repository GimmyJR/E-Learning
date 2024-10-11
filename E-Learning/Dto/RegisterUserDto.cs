using System.ComponentModel.DataAnnotations;

namespace E_Learning.Dto
{
    public class RegisterUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string AvatarUrl { get; set; }
        public string Bio { get; set; }
    }
}
