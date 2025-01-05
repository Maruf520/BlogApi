using Blog.Models.UserModel;

namespace Blog.Dtos.Users
{
    public class LoginResponse
    {
        public LoginResponse()
        {
            User = new UserDto();
        }
        public string Token { get; set; }
        public int ExpiresInSeconds { get; set; }
        public UserDto User { get; set; }
    }
}
