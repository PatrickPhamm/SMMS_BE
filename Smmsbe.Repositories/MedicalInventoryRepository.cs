using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;
using Smmsbe.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Repositories
{
    public class MedicalInventoryRepository : Repository<MedicalInventory>, IMedicalInventoryRepository
    {
        public MedicalInventoryRepository(SMMSContext _context) : base(_context) { }

        public override async Task<MedicalInventory> GetById(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.MedicalInventoryId == id);
        }
    }
}
