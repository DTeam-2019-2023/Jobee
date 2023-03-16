using System;
using System.Collections.Generic;

namespace Jobee_API.Entities
{
    public partial class Project
    {
        public string Id { get; set; } = null!;
        public string Idcv { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int? TeamSize { get; set; }
        public string Role { get; set; } = null!;
        public string Technology { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }

        public virtual TbCv IdcvNavigation { get; set; } = null!;
    }
}
