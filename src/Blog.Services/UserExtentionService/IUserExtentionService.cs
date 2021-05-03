using Blog.Models.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.UserExtentionService
{
    public interface IUserExtentionService
    {
        Task<string> GenerateAccessToken(User user);
    }
}
