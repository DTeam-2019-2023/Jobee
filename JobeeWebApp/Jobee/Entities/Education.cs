using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jobee_API.Entities
{
    public partial class Education
    {
        public string Id { get; set; } = null!;
        public string Idcv { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Major { get; set; } = null!;
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public double Gpa { get; set; }
        public string? Description { get; set; }

        public virtual TbCv IdcvNavigation { get; set; } = null!;
    }
}
