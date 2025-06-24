using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;
using Smmsbe.Repositories.Interfaces;

namespace Smmsbe.Repositories
{
    public class FormRepository : Repository<Form>, IFormRepository
    {
        public FormRepository(SMMSContext _context) : base(_context) { }

        public override async Task<Form> GetById(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.FormId == id);
        }
    }
}
