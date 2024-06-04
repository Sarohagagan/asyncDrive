using Microsoft.EntityFrameworkCore;
using asyncDrive.API.Repositories.IRepository;
using asyncDrive.DataAccess;
using asyncDrive.Models.Domain;

namespace asyncDrive.API.Repositories
{
    public class WebsitePageRepository : Repository<Websitepage>, IWebsitePageRepository
    {
        private readonly asyncDriveDbContext dbContext;
        public WebsitePageRepository(asyncDriveDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task UpdateAsync(Websitepage obj)
        {
            await Task.Run(() =>
            {
                dbContext.Update(obj);
            });
        }
    }
}
