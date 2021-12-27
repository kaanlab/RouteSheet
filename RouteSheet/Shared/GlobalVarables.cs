﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Shared
{
    public class GlobalVarables
    {
        public static readonly SymmetricSecurityKey KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sOmeran0meString!"));
        public static class Roles
        {
            public const string ADMIN = "admin";
            public const string TEACHER = "teacher";
            public const string MEDIC = "medic";
        }
    }
}
