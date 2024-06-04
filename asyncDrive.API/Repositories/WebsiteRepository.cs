using Microsoft.EntityFrameworkCore;
using asyncDrive.API.Repositories.IRepository;
using asyncDrive.DataAccess;
using asyncDrive.Models.Domain;

namespace asyncDrive.API.Repositories
{
    public class WebsiteRepository : Repository<Website>, IWebsiteRepository
    {
        private readonly asyncDriveDbContext dbContext;
        public WebsiteRepository(asyncDriveDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task UpdateAsync(Website obj)
        {
            await Task.Run(() =>
            {
                dbContext.Update(obj);
            });
        }
    }
}
