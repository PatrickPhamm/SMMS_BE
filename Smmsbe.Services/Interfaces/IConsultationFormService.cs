using Smmsbe.Repositories.Entities;
using Smmsbe.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Interfaces
{
    public interface IConsultationFormService
    {
        Task<ConsultationForm> GetById(int id);
        Task<ConsultationFormResponse> GetByIdAsync(int id);
        Task<List<GetConsultationFormByParentResponse>> GetByParent(int parentId);
        Task<List<GetConsultationFormByStudentResponse>> GetByStudent(int studentId);
        Task<ConsultationForm> AddConsultationFormAsync(AddConsultationFormRequest request);
        Task<List<SearchConsultationFormResponse>> SearchConsultationFormAsync(SearchConsultationFormRequest request);
        Task<ConsultationForm> UpdateConsultationFormAsync(UpdateConsultationFormRequest request);
        Task<bool> DeleteConsultationFormAsync(int id);
        Task<bool> AcceptConsultation(int consultationId);
        Task<bool> RejectConsultation(int consultationId);
    }
}
