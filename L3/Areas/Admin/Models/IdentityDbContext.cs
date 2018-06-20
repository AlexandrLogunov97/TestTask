﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace L3.Areas.Admin.Models
{
    public class IdentityDB: IdentityDbContext<User>
    {
        public IdentityDB() : base("IdentityDB") { }

        public static IdentityDB Create()
        {
            return new IdentityDB();
        }
    }
}