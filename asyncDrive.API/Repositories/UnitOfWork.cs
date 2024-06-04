using asyncDrive.API.Repositories.IRepository;
using asyncDrive.DataAccess;



namespace asyncDrive.API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private asyncDriveDbContext dbContext;
        public IUserRepository User { get; private set; }
        public IWebsiteRepository Website { get; private set; }
        public IWebsiteDataRepository WebsiteData { get; private set; }
        public IWebsitePageRepository WebsitePage { get; private set; }

        public UnitOfWork(asyncDriveDbContext dbContext)
        {
            this.dbContext = dbContext;
            User = new UserRepository(dbContext);
            Website = new WebsiteRepository(dbContext);
            WebsiteData = new WebsiteDataRepository(dbContext);
            WebsitePage = new WebsitePageRepository(dbContext);
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
