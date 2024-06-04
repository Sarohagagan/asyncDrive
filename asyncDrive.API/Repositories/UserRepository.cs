using Microsoft.EntityFrameworkCore;
using asyncDrive.API.Repositories.IRepository;
using asyncDrive.DataAccess;
using asyncDrive.Models.Domain;
using asyncDrive.Models.DTO;

namespace asyncDrive.API.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly asyncDriveDbContext dbContext;
        public UserRepository(asyncDriveDbContext dbContext) : base(dbContext)
        {
                this.dbContext = dbContext;
        }

        public async Task UpdateAsync(User obj)
        {
            await Task.Run(() =>
            {
                dbContext.Update(obj);
            });
        }
    }
}
