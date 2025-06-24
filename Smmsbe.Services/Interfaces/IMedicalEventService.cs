using Smmsbe.Repositories.Entities;
using Smmsbe.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Interfaces
{
    public interface IMedicalEventService
    {
        Task<MedicalEvent> GetById(int id);
        Task<List<MedicalEventResponse>> GetMedicalByStudent(int studentId);
        Task<MedicalEvent> AddMedicalEventAsync(AddMedicalEventRequest request);
        Task<List<MedicalEventResponse>> SearchMedicalEventAsync(SearchMedicalEventRequest request);
        Task<MedicalEvent> UpdateMedicalEventAsync(UpdateMedicalEventRequest request);
        Task<bool> DeleteMedicalEventAsync(int id);
    }
}
