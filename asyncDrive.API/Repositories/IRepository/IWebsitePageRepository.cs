using Models.DTO;
using Models.Domain;

namespace asyncDrive.API.Repositories.IRepository
{
    public interface IWebsitePageRepository:IRepository<Websitepage>
    {
        Task UpdateAsync(Websitepage obj);
    }
}
