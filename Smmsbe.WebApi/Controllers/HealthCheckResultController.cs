using Microsoft.AspNetCore.Mvc;
using Smmsbe.Services;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;

namespace Smmsbe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class healthCheckResultController : ControllerBase
    {
        private readonly IHealthCheckResultService _healthCheckResultService;
        public healthCheckResultController(IHealthCheckResultService healthCheckResultService)
        {
            _healthCheckResultService = healthCheckResultService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var getById = await _healthCheckResultService.GetById(id);
            return Ok(getById);
        }

        [HttpGet("getResultsBySchedule{scheduleId}")]
        public async Task<IActionResult> GetResultsBySchedule(int scheduleId)
        {
            var getId = await _healthCheckResultService.GetResultsBySchedule(scheduleId);
            return Ok(getId);
        }

        [HttpGet("getResultsByProfile{profileId}")]
        public async Task<IActionResult> GetResultsByProfile(int profileId)
        {
            var getId = await _healthCheckResultService.GetResultsByHealthProfile(profileId);
            return Ok(getId);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddHealthCheckResult(AddHealthCheckResultRequest request)
        {
            var addHealthCheckResult = await _healthCheckResultService.AddHealthCheckResultAsync(request);
            return Ok(addHealthCheckResult);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(SearchHealthCheckResultRequest request)
        {
            var result = await _healthCheckResultService.SearchHealthCheckResultAsync(request);

            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateHealthCheckResult(UpdateHealthCheckResultRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updateHealthCheckResult = await _healthCheckResultService.UpdateHealthCheckResultAsync(request);
            return Ok(updateHealthCheckResult);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHealthCheckResult(int id)
        {
            var deleteHealthCheckResult = await _healthCheckResultService.DeleteHealthCheckResultAsync(id);
            return Ok();
        }

        [HttpPost("complete/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _healthCheckResultService.CompleteCheckResultAsync(id);

            return Ok(result);
        }
    }
}
