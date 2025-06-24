using Smmsbe.Repositories.Entities;
using Smmsbe.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
