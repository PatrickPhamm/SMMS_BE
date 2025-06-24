using Smmsbe.Repositories.Entities;
using Smmsbe.Services.Models;

namespace Smmsbe.Services.Interfaces
{
    public interface IHealthProfileService 
    {
        Task<HealthProfile> GetById(int id);

        Task<HealthProfile> AddHealthProfileAsync(AddHealthProfileRequest request);

        Task<List<HealthProfileResponse>> SearchHealthProfileAsync(SearchHealthProfileRequest request);

        Task<HealthProfile> UpdateHealthProfileAsync(UpdateHealthProfileRequest request);

        Task<bool> DeleteHealthProfileAsync(int id);
    }
}
