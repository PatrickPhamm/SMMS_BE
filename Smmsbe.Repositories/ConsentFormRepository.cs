using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;
using Smmsbe.Repositories.Interfaces;

namespace Smmsbe.Repositories
{
    public class ConsentFormRepository : Repository<ConsentForm>, IConsentFormRepository
    {
        public ConsentFormRepository(SMMSContext _context) : base(_context) { }

        public override async Task<ConsentForm> GetById(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.ConsentFormId == id);
        }
    }
}
