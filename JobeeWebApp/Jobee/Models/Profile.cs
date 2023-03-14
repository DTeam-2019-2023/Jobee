using System.ComponentModel.DataAnnotations;

namespace Jobee_API.Models
{
    public class Profile
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public bool Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime DoB { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string SocialNetwork { get; set; } = null!;
        public string DetailAddress { get; set; } = null!;
        public string? Email { get; set; }
    }
}
