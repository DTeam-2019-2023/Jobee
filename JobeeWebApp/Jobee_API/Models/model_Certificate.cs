namespace Jobee_API.Models
{
    public class model_Certificate
    {
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Url { get; set; } = null!;
        public string? Description { get; set; }
        public bool? IsVertify { get; set; }
    }
}
