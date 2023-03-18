namespace Jobee_API.Models
{
    public class CV
    {
        //public string id_employee { get; set; }
        public string ApplyPosition { get; set; }
        public string CurrentJob { get; set; }
        public decimal DesirySalary { get; set; }
        public string Degree { get; set; }
        public string WorkExperience { get; set; }
        public string DesiredWorkLocation { get; set; }
        public string WorkingForm { get; set; }
        public string CarrerObjiect { get; set; }
        public string SoftSkill { get; set; }
        public string Avatar { get; set; }

        public CV()
        {
            this.ApplyPosition = string.Empty;
            this.CurrentJob = string.Empty;
            this.DesirySalary = 0;
            this.Degree = string.Empty;
            this.WorkExperience = string.Empty;
            this.DesiredWorkLocation = string.Empty;
            this.WorkingForm = string.Empty;
            this.CarrerObjiect = string.Empty;
            this.SoftSkill = string.Empty;
            this.Avatar = string.Empty;
        }
    }
}
