using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;
using Smmsbe.Repositories.Interfaces;

namespace Smmsbe.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(SMMSContext _context) : base(_context) { }

        public override async Task<Student> GetById(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.StudentId == id);
        }

        public async Task<bool> StudentIdExsistAsync(int id)
        {
            return await Table.AnyAsync(y => y.StudentId == id);
        }
    }
}
