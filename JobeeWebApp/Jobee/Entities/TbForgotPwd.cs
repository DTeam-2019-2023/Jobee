﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jobee_API.Entities
{
    public partial class TbForgotPwd
    {
        public int Id { get; set; }
        public string Uid { get; set; } = null!;
        public string Link { get; set; } = null!;
        [DataType(DataType.Date)]
        public DateTime ExpireDay { get; set; }

        public virtual TbAccount UidNavigation { get; set; } = null!;
    }
}
