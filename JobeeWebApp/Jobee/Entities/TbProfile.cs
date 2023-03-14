using System;
using System.Collections.Generic;

namespace Jobee_API.Entities
{
    public partial class TbProfile
    {
        public TbProfile()
        {
            TbAdmins = new HashSet<TbAdmin>();
            TbEmployees = new HashSet<TbEmployee>();
        }

        public string Id { get; set; } = null!;
        public string Idaccount { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public bool Gender { get; set; }
        public DateTime DoB { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string SocialNetwork { get; set; } = null!;
        public string DetailAddress { get; set; } = null!;
        public string? Email { get; set; }

        public virtual TbAccount IdaccountNavigation { get; set; } = null!;
        public virtual ICollection<TbAdmin> TbAdmins { get; set; }
        public virtual ICollection<TbEmployee> TbEmployees { get; set; }
    }
}
