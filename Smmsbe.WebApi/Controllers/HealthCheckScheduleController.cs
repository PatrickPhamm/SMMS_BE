using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smmsbe.Services;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;

namespace Smmsbe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class healthCheckScheduleController : ControllerBase
    {
        private readonly IHealthCheckScheduleService _healthCheckScheduleService;
        public healthCheckScheduleController(IHealthCheckScheduleService healthCheckScheduleService)
        {
            _healthCheckScheduleService = healthCheckScheduleService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var getById = await _healthCheckScheduleService.GetById(id);
            return Ok(getById);
        }

        [HttpGet("getByForm{formId}")]
        public async Task<IActionResult> GetByFrom(int formId)
        {
            var getById = await _healthCheckScheduleService.GetByForm(formId);
            return Ok(getById);
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddHealthCheckSchedule(AddHealthCheckScheduleRequest request)
        {
            var addHealthCheckSchedule = await _healthCheckScheduleService.AddHealthCheckScheduleAsync(request);
            return Ok(addHealthCheckSchedule);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(SearchHealthCheckScheduleRequest request)
        {
            var result = await _healthCheckScheduleService.SearchHealthCheckScheduleAsync(request);

            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateHealthCheckSchedule(UpdateHealthCheckScheduleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updateHealthCheckSchedule = await _healthCheckScheduleService.UpdateHealthCheckScheduleAsync(request);
            return Ok(updateHealthCheckSchedule);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHealthCheckSchedule(int id)
        {
            var deleteHealthCheckSchedule = await _healthCheckScheduleService.DeleteHealthCheckScheduleAsync(id);
            return Ok();
        }
    }
}
