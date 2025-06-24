using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;

namespace Smmsbe.Repositories.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<bool> StudentIdExsistAsync(int id);
    }
}
