using System.ComponentModel.DataAnnotations;

namespace Jobee_API.Models
{
    public class model_Education
    {
        public string Name { get; set; }
        public string Major { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [Range(0, 4)]
        public double GPA { get; set; }
        public string Description { get; set; }
    }
}
