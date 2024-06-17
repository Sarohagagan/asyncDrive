using Models.Domain;

namespace asyncDrive.API.Repositories.IRepository
{
    public interface IWebsiteDataRepository:IRepository<Websitedata>
    {
        Task UpdateAsync(Websitedata obj);
    }
}
