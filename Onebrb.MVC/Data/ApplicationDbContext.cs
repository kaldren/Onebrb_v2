using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Onebrb.MVC.Areas.Manager.Models;
using Onebrb.MVC.Models;

namespace Onebrb.MVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // One company can have many jobs and every job is owned by single company
            builder.Entity<Job>()
                .HasOne(c => c.Company)
                .WithMany(j => j.Jobs)
                .HasForeignKey(f => f.CompanyId);

            builder.Entity<ApplicationUserJob>()
                .HasKey(bc => new { bc.JobId, bc.ApplicationUserId });
            builder.Entity<ApplicationUserJob>()
                .HasOne(bc => bc.Job)
                .WithMany(b => b.ApplicationUserJob)
                .HasForeignKey(bc => bc.JobId);
            builder.Entity<ApplicationUserJob>()
                .HasOne(bc => bc.ApplicationUser)
                .WithMany(c => c.ApplicationUserJob)
                .HasForeignKey(bc => bc.ApplicationUserId);
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Job> Jobs { get; set; }
    }
}
