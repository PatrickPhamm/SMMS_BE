using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;
using Smmsbe.Repositories.Interfaces;

namespace Smmsbe.Repositories
{
    public class ParentRepository : Repository<Parent>, IParentRepository
    {
        public ParentRepository(SMMSContext _context) : base(_context) { }

        public override async Task<Parent> GetById(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.ParentId == id);
        }

        public async Task<bool> ParentIdExsistAsync(int id)
        {
            return await Table.AnyAsync(y => y.ParentId == id);
        }
    }
}
