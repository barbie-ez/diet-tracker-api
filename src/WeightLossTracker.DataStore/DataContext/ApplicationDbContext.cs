using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WeightLossTracker.DataStore.Entitties;

namespace WeightLossTracker.DataStore
{
    public class ApplicationDbContext : IdentityDbContext<UserProfileModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }

        public DbSet<UserProfileModel> Users { get; set; }
        public DbSet<UserRoleModel> Roles { get; set; }


    }
}
