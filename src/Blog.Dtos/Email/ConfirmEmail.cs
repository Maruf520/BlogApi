using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Dtos.Email
{
    public class ConfirmEmail
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
