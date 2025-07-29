using Smmsbe.Repositories.Entities;
using Smmsbe.Services.Models;

namespace Smmsbe.Services.Interfaces
{
    public interface IStudentService
    {
        Task<Student> GetById(int id);
        Task<StudentResponse> GetByIdAsync(int id);
        Task<List<StudentResponse>> GetStudentByParent(int parentId);
        Task<Student> AuthorizeAsync(string studentNumber, string password);
        Task<Student> AddStudentAsync(AddStudentRequest request);
        Task<UpdateStudentReponse> UpdateStudentAsync(UpdateStudentRequest request);
        Task<bool> DeleteStudentAsync(int id);
        Task<List<StudentResponse>> SearchStudentAsync(SearchStudentRequest request);
    }
}
