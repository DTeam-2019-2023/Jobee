using System.ComponentModel.DataAnnotations;

namespace Jobee_API.Models
{
    public class User
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}
