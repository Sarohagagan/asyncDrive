using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace asyncDrive.DataAccess
{
    public class asyncDriveAuthDbContext : IdentityDbContext
    {
        public asyncDriveAuthDbContext(DbContextOptions<asyncDriveAuthDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //var siteAdminRoleId = "d9b5353e-3c0e-4fc4-9c8b-57291c410a15";
            //var superAdminRoleId = "1e7842ae-ef57-4d7b-889a-950789a621c2";
            //var userRoleId = "0ff37f2f-601b-4924-9bc5-28923936b6a7";
            //var viewRoleId = "2a35652a-6d71-4188-9d50-4dc76b14f298";
            //var createRoleId = "00493d86-007f-4cbf-95cc-0d3fb445a4db";
            //var editRoleId = "913b14eb-9c92-449b-af81-50b254fc3d9a";
            //var deleteRoleId = "621e6d91-8765-43f4-b966-a5ea786d29f3";

            //var roles = new List<IdentityRole>
            //{
            //    new IdentityRole
            //    {
            //        Id = siteAdminRoleId,
            //        ConcurrencyStamp=siteAdminRoleId,
            //        Name= "SiteAdmin",
            //        NormalizedName="SiteAdmin".ToUpper(),
            //    },
            //    new IdentityRole
            //    {
            //        Id = superAdminRoleId,
            //        ConcurrencyStamp=superAdminRoleId,
            //        Name= "SuperAdmin",
            //        NormalizedName="SuperAdmin".ToUpper(),
            //    },
            //    new IdentityRole
            //    {
            //        Id = userRoleId,
            //        ConcurrencyStamp=userRoleId,
            //        Name= "User",
            //        NormalizedName="User".ToUpper(),
            //    },
            //    new IdentityRole
            //    {
            //        Id = viewRoleId,
            //        ConcurrencyStamp=viewRoleId,
            //        Name= "View",
            //        NormalizedName="View".ToUpper(),
            //    },
            //    new IdentityRole
            //    {
            //        Id = createRoleId,
            //        ConcurrencyStamp=createRoleId,
            //        Name= "Create",
            //        NormalizedName="Create".ToUpper(),
            //    },
            //    new IdentityRole
            //    {
            //        Id = editRoleId,
            //        ConcurrencyStamp=editRoleId,
            //        Name= "Edit",
            //        NormalizedName="Edit".ToUpper(),
            //    },
            //    new IdentityRole
            //    {
            //        Id = deleteRoleId,
            //        ConcurrencyStamp=deleteRoleId,
            //        Name= "Delete",
            //        NormalizedName="Delete".ToUpper(),
            //    }
            //};

            //builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
