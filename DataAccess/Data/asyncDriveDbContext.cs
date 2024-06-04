using Microsoft.EntityFrameworkCore;
using asyncDrive.Models.Domain;

namespace asyncDrive.DataAccess
{
    public class asyncDriveDbContext : DbContext
    {
        public asyncDriveDbContext(DbContextOptions<asyncDriveDbContext> dbContextOptions):base(dbContextOptions)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Website> Websites { get; set; }
        public DbSet<Websitedata> Websitedata { get; set; }
        public DbSet<Websitepage> Websitepages { get; set; }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ////Seed data for User
            //var users = new List<User>()
            //{
            //    new User
            //    {
            //        Id=Guid.Parse("d7a6483c-d554-4a1a-ab74-65e7a4c82c96"),
            //        FirstName="Gagan",
            //    },
            //    new User
            //    {
            //        Id=Guid.Parse("d7a6483c-d554-4a1a-ab74-65e7a4c82c96"),
            //        FirstName="test",
            //    }
            //};

            ////seed user to the database
            //modelBuilder.Entity<User>().HasData(users);
        }
    }
}
