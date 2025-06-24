using Smmsbe.Repositories.Entities;
using Smmsbe.Services.Models;

namespace Smmsbe.Services.Interfaces
{
    public interface IMedicalInventoryService
    {
        Task<MedicalInventory> GetById(int id);

        Task<MedicalInventory> AddMedicalInventoryAsync(AddMedicalInventoryRequest request);

        Task<MedicalInventory> UpdateMedicalInventoryAsync(UpdateMedicalInventoryRequest request);

        Task<bool> DeleteMedicalInventoryAsync(int id);

        Task<List<MedicalInventoryResponse>> SearchMedicalInventorysAsync(SearchMedicalInventoryRequest request);
    }
}
