using System;
using System.Collections.Generic;

namespace Jobee_API.Entities
{
    public partial class TbAccount
    {
        public TbAccount()
        {
            TbCvs = new HashSet<TbCv>();
            TbForgotPwds = new HashSet<TbForgotPwd>();
            TbProfiles = new HashSet<TbProfile>();
        }

        public string Id { get; set; } = null!;
        public string IdtypeAccount { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Passwork { get; set; } = null!;

        public virtual TbTypeAccount IdtypeAccountNavigation { get; set; } = null!;
        public virtual ICollection<TbCv> TbCvs { get; set; }
        public virtual ICollection<TbForgotPwd> TbForgotPwds { get; set; }
        public virtual ICollection<TbProfile> TbProfiles { get; set; }
    }
}
