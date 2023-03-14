using System.ComponentModel.DataAnnotations;

namespace Jobee_API.Models
{
    public class model_Certificate
    {
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }
}
