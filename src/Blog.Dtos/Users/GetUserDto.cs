using Blog.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Dtos.Users
{
   public class GetUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string ProfilePic { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public DateTime LastDateOfDonation { get; set; }
        public string Email { get; set; }
    }
}
