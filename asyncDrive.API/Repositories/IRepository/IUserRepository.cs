using Models.Domain;


namespace asyncDrive.API.Repositories.IRepository
{
    public interface IUserRepository:IRepository<User>
    {
        Task UpdateAsync(User obj);
    }
}
