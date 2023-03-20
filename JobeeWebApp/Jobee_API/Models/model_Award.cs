namespace Jobee_API.Models
{
    public class model_Award
    {
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public string? Role { get; set; }
    }
}
