using System;
using System.Collections.Generic;

namespace Jobee_API.Entities
{
    public partial class Education
    {
        public string Id { get; set; } = null!;
        public string Idcv { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Major { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Gpa { get; set; }
        public string? Description { get; set; }

        public virtual TbCv IdcvNavigation { get; set; } = null!;
    }
}
