using Smmsbe.Repositories.Entities;
using Smmsbe.Services.Models;

namespace Smmsbe.Services.Interfaces
{
    public interface IParentPrescriptionService 
    {
        Task<ParentPrescription> GetById(int id);
        Task<List<ParentPrescriptionResponse2>> GetPrescriptionByParent(int parentId);
        Task<ParentPrescription> AddParentPrescriptionAsync(AddParentPrescriptionRequest request);
        Task<List<ParentPrescriptionResponse>> SearchParentPrescriptionAsync(SearchParentPrescriptionRequest request);
        Task<ParentPrescription> UpdateParentPrescriptionAsync(UpdateParentPrescriptionRequest request);
        Task<bool> DeleteParentPrescriptionAsync(int id);
        string GetImageFolder();
        Task<bool> AcceptPrescription(int prescriptionId);
        Task<bool> RejectPrescription(int prescriptionId);
    }
}
