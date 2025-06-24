using Smmsbe.Repositories.Entities;
using Smmsbe.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Interfaces
{
    public interface IMedicationService
    {
        Task<Medication> GetById(int id);
        Task<MedicationResponse> GetByIdAsync(int id);
        Task<List<MedicationResponse>> GetMedicalByPrescription(int prescriptionId);
        Task<List<MedicationByStudentResponse>> GetMedicalByStudent(int studentId);
        Task<Medication> AddMedicationAsync(AddMedicationRequest request);
        Task<List<MedicationResponse>> SearchMedicationAsync(SearchMedicationRequest request);
        Task<Medication> UpdateMedicationAsync(UpdateMedicationRequest request);
        Task<bool> DeleteMedicationAsync(int id);
    }
}
