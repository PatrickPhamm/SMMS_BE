using Smmsbe.Repositories.Entities;
using Smmsbe.Services.Models;

namespace Smmsbe.Services.Interfaces
{
    public interface IFormService
    {
        Task<FormResponse> GetById(int id);
        //Task<FormResponse> AddFormAsync(AddFormRequest request);
        Task<FormResponseAdded> AddFormAsync(AddFormRequest request);
        Task<FormResponse> UpdateFormAsync(UpdateFormRequest request);
        Task<List<FormResponse>> SearchFormAsync(SearchFormRequest request);
        Task<bool> DeleteFormAsync(int id);
    }
}
