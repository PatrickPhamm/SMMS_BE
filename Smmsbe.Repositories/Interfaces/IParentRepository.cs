using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;

namespace Smmsbe.Repositories.Interfaces
{
    public interface IParentRepository : IRepository<Parent>
    {
        Task<bool> ParentIdExsistAsync(int id);
    }
}
