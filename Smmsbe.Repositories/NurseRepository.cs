using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;
using Smmsbe.Repositories.Interfaces;

namespace Smmsbe.Repositories
{
    public class NurseRepository : Repository<Nurse>, INurseRepository
    {
        public NurseRepository(SMMSContext _context) : base(_context) { }

        public override async Task<Nurse> GetById(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.NurseId == id);
        }

        public async Task<bool> NurseIdExsistAsync(int id)
        {
            return await Table.AnyAsync(y => y.NurseId == id);
        }
    }
}
