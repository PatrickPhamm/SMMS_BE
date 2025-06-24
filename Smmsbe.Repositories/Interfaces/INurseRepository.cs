using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;

namespace Smmsbe.Repositories.Interfaces
{
    public interface INurseRepository : IRepository<Nurse>
    {
        Task<bool> NurseIdExsistAsync(int id);
    }
}
