﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Resources.AccountDtos
{
    public class UserDto
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}
