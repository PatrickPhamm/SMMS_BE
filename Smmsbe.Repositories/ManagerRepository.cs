using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;
using Smmsbe.Repositories.Interfaces;

namespace Smmsbe.Repositories
{
    public class ManagerRepository : Repository<Manager>, IManagerRepository
    {
        public ManagerRepository(SMMSContext _context) : base(_context) { }

        public override async Task<Manager> GetById(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.ManagerId == id);
        }

        public async Task<bool> ExistsByIdAsync(int managerId)
        {
            return await Table.AnyAsync(o => o.ManagerId == managerId);
        }
    }
}
