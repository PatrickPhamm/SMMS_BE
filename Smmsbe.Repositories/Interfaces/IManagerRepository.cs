using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;

namespace Smmsbe.Repositories.Interfaces
{
    public interface IManagerRepository : IRepository<Manager>
    {
        Task<bool> ExistsByIdAsync(int mangerId);
    }
}
