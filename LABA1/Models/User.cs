﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LABA1.Models
{
    public class User : IdentityUser
    {
        public string Nickname { get; set; }
    }
}
