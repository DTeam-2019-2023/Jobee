using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Jobee_API.Models
{
    public class Admin
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string rePassword { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime dob { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string email { get; set; }
        public string DetailAddress { get; set; }

    }
}
