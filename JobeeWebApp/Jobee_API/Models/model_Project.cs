using System.Security.Policy;

namespace Jobee_API.Models
{
    public class model_Project
    {
        public string Name { get; set; }
        public int TeamSize { get; set; }
        public string Role { get; set; }
        public string Technology { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; } 
    }
}
