using System.ComponentModel.DataAnnotations;

namespace Jobee_API.Models
{
    public class model_Award
    {
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string? Role { get; set; }
    }
}
