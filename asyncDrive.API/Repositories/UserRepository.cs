using Microsoft.EntityFrameworkCore;
using asyncDrive.API.Repositories.IRepository;
using asyncDrive.DataAccess;
using Models.DTO;
using Models.Domain;

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
