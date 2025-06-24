using Smmsbe.Repositories.Entities;
using Smmsbe.Services.Models;

namespace Smmsbe.Services.Interfaces
{
    public interface IConsentFormService
    {
        Task<ConsentForm> GetById(int id);
        Task<ConsentFormResponse> GetByIdAsync(int id);
        Task<List<ConsentFormResponse>> GetConsentFormByParent(int parentId);
        Task<List<ConsentFormByStudentResponse>> GetAcceptedByStudent(GetConsentFromRequest request);
        Task<ConsentForm> AddConsentFormAsync(AddConsentFormRequest request);
        Task<List<ConsentFormResponse>> SearchConsentFormAsync(SearchConsentFormRequest request);
        Task<ConsentForm> UpdateConsentFormAsync(UpdateConsentFormRequest request);
        Task<bool> AcceptConsentForm(int consentFormId);
        Task<bool> RejectConsentForm(int consentFormId);
        Task<bool> DeleteConsentFormAsync(int id);
    }
}
