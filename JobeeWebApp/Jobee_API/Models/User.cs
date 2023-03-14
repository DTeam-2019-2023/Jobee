using Microsoft.Build.Framework;

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
