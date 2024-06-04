using asyncDrive.API.Repositories.IRepository;

namespace asyncDrive.API.Repositories.IRepository
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        IWebsiteRepository Website { get; }

        IWebsiteDataRepository WebsiteData { get; }
        IWebsitePageRepository WebsitePage { get; }

        Task SaveAsync();
    }
}
