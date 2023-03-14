using System;
using System.Collections.Generic;

namespace Jobee_API.Entities
{
    public partial class TbTypeAccount
    {
        public TbTypeAccount()
        {
            TbAccounts = new HashSet<TbAccount>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<TbAccount> TbAccounts { get; set; }
    }
}
