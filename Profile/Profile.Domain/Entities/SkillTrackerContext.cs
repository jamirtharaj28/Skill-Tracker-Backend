﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Domain.Entities
{
    public class SkillTrackerContext : DbContext
    {
        public SkillTrackerContext(DbContextOptions<SkillTrackerContext> option):base (option)
        {
              
        }

        public DbSet<ProfileEntity> Profile { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<ProfileEntity>( p=> 
                {
                    p.ToContainer("SkillTrackerContainer");
                    p.HasKey(x => x.EmpId);
                    //p.HasPartitionKey(x => x.skills.Select(s=>s.Name));
                    p.OwnsMany(s => s.skills);
                });
        }
    }
}
