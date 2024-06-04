using asyncDrive.Models.Domain;

namespace asyncDrive.API.Repositories.IRepository
{
    public interface IWebsiteRepository : IRepository<Website>
    {
        Task UpdateAsync(Website obj);
    }
}
