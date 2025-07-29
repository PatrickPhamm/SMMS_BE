using Smmsbe.Repositories.Entities;
using Smmsbe.Services.Models;

namespace Smmsbe.Services.Interfaces
{
    public interface IParentService
    {
        Task<Parent> GetById(int id);
        Task<List<StudentResponse>> GetParentFromStudent(int studentId);
        Task<Parent> AuthorizeAsync(string username, string password);
        Task<ParentResponse> UpdateParentAsync(UpdateParentRequest request);
        Task<Parent> AddParentAsync(AddParentRequest request);
        Task<bool> DeleteParentAsync(int id);
        Task<List<ParentResponse>> SearchParentAsync(SearchParentRequest request);
        //Task<Parent> GetAllAsync();

        Task<bool> ApproveParentAsync(int parentId);
        Task<bool> ActivateAccountAsync(string code);
    }
}
