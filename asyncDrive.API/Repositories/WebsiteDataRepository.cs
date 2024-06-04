using Microsoft.EntityFrameworkCore;
using asyncDrive.API.Repositories.IRepository;
using asyncDrive.DataAccess;
using asyncDrive.Models.Domain;


namespace asyncDrive.API.Repositories
{
    public class WebsiteDataRepository : Repository<Websitedata>, IWebsiteDataRepository
    {
        private readonly asyncDriveDbContext dbContext;
        public WebsiteDataRepository(asyncDriveDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task UpdateAsync(Websitedata obj)
        {
            await Task.Run(() =>
            {
                dbContext.Update(obj);
            });
        }
    }
}
