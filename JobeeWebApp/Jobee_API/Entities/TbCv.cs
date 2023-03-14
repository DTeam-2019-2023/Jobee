using System;
using System.Collections.Generic;

namespace Jobee_API.Entities
{
    public partial class TbCv
    {
        public TbCv()
        {
            Activities = new HashSet<Activity>();
            Awards = new HashSet<Award>();
            Certificates = new HashSet<Certificate>();
            Educations = new HashSet<Education>();
            Projects = new HashSet<Project>();
        }

        public string Id { get; set; } = null!;
        public string Idaccount { get; set; } = null!;
        public string ApplyPosition { get; set; } = null!;
        public string CurrentJob { get; set; } = null!;
        public decimal DesirySalary { get; set; }
        public string Degree { get; set; } = null!;
        public string WorkExperience { get; set; } = null!;
        public string DesiredWorkLocation { get; set; } = null!;
        public string WorkingForm { get; set; } = null!;
        public string CarrerObject { get; set; } = null!;
        public string? SoftSkill { get; set; }
        public string? Avatar { get; set; }

        public virtual TbAccount IdaccountNavigation { get; set; } = null!;
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Award> Awards { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
