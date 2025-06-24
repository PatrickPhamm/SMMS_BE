using Smmsbe.Repositories.Entities;
using Smmsbe.Services.Models;


namespace Smmsbe.Services.Interfaces
{
    public interface IManagerService
    {
        Task<Manager> GetById(int id);

        Task<Manager> AuthorizeAsync(string email, string password);

        /*Task<Manager> RegisterManagerAsync(RegisterManagerRequest request);*/

        Task<Manager> UpdateManagerAsync(UpdateManagerRequest request);
    }
}
