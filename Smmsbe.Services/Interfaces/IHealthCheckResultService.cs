using Smmsbe.Repositories.Entities;
using Smmsbe.Services.Models;

namespace Smmsbe.Services.Interfaces
{
    public interface IHealthCheckResultService
    {
        Task<HealthCheckResultResponse> GetById(int id);
        Task<HealthCheckResultResponse> AddHealthCheckResultAsync(AddHealthCheckResultRequest request);
        Task<List<HealthCheckResultResponse>> GetResultsBySchedule(int scheduleId);
        Task<List<GetHealthCheckResultByProfileResponse>> GetResultsByHealthProfile(int profileId);
        Task<List<HealthCheckResultResponse>> SearchHealthCheckResultAsync(SearchHealthCheckResultRequest request);
        Task<HealthCheckResult> UpdateHealthCheckResultAsync(UpdateHealthCheckResultRequest request);
        Task<bool> DeleteHealthCheckResultAsync(int id);
        Task<bool> CompleteCheckResultAsync(int id);
    }
}
