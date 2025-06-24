using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;
using Smmsbe.Repositories.Interfaces;

namespace Smmsbe.Repositories
{
    public class ConsultationFormRepository : Repository<ConsultationForm>, IConsultationFormRepository
    {
        public ConsultationFormRepository(SMMSContext _context) : base(_context) { }

        public override async Task<ConsultationForm> GetById(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.ConsultationFormId == id);
        }
    }
}
