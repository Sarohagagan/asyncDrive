using Models.Domain;

namespace asyncDrive.API.Repositories.IRepository
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
