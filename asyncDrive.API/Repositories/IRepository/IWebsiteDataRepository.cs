using asyncDrive.Models.Domain;
using asyncDrive.Models.DTO;

namespace asyncDrive.API.Repositories.IRepository
{
    public interface IWebsiteDataRepository:IRepository<Websitedata>
    {
        Task UpdateAsync(Websitedata obj);
    }
}
