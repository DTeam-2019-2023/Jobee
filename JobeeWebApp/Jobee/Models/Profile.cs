using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Jobee_API.Models
{
    public class Profile
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name invalid")]
        public string LastName { get; set; } = null!;
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name invalid")]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage ="Please select your gender")]
        public bool Gender { get; set; } = true;
        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Day of Birth")]
        public DateTime DoB { get; set; }
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Number phone not valid")]
        public string PhoneNumber { get; set; } = null!;
        [RegularExpression(@"^[\p{L}\d\s\-\.,/\\]+$", ErrorMessage = "Address not valid")]
        public string Address { get; set; } = null!;
        [RegularExpression(@"^(https?:\/\/)?(www\.)?(facebook|twitter|linkedin|instagram)\.com\/[a-zA-Z0-9(\.\?)?]+$", ErrorMessage = "Your social network not valid")]
        public string? SocialNetwork { get; set; } = string.Empty;
        [RegularExpression(@"^[\p{L}\d\s\-\.,/\\]+$", ErrorMessage = "Address not valid")]
        public string DetailAddress { get; set; } = null!;
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Your mail not valid")]
        public string Email { get; set; }
    }
}
