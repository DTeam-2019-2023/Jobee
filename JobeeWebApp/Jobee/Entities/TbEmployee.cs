using System;
using System.Collections.Generic;

namespace Jobee_API.Entities
{
    public partial class TbEmployee
    {
        public string Id { get; set; } = null!;
        public string Idprofile { get; set; } = null!;

        public virtual TbProfile IdprofileNavigation { get; set; } = null!;
    }
}
