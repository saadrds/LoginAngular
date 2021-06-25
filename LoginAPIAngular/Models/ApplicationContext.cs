﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginAPIAngular.Models
{
    public class ApplicationContext:DbContext
    {
   
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {

        }
        public DbSet <User> Users { get; set; }

    }
}
