using Smmsbe.Repositories.Entities;
using Smmsbe.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Interfaces
{
    public interface IHealthCheckScheduleService
    {
        Task<HealthCheckSchedule> GetById(int id);
        Task<List<GetHealthCheckScheduleByFormResponse>> GetByForm(int formId);
        Task<List<HealthCheckScheduleResponse>> SearchHealthCheckScheduleAsync(SearchHealthCheckScheduleRequest request);
        Task<HealthCheckSchedule> AddHealthCheckScheduleAsync(AddHealthCheckScheduleRequest request);
        Task<HealthCheckSchedule> UpdateHealthCheckScheduleAsync(UpdateHealthCheckScheduleRequest request);
        Task<bool> DeleteHealthCheckScheduleAsync(int id);
    }
}
