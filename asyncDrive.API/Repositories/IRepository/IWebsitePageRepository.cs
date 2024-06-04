using asyncDrive.Models.Domain;
using asyncDrive.Models.DTO;

namespace asyncDrive.API.Repositories.IRepository
{
    public interface IWebsitePageRepository:IRepository<Websitepage>
    {
        Task UpdateAsync(Websitepage obj);
    }
}
