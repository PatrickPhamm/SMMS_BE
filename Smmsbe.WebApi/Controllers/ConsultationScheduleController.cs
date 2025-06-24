using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smmsbe.Services;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;

namespace Smmsbe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class consultationScheduleController : ControllerBase
    {
        private readonly IConsultationScheduleService _consultationScheduleService;
        public consultationScheduleController(IConsultationScheduleService consultationScheduleService)
        {
            _consultationScheduleService = consultationScheduleService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var getById = await _consultationScheduleService.GetByIdAsync(id);
            return Ok(getById);
        }

        [HttpGet("getByStudent")]
        public async Task<IActionResult> GetByStudent(int studentId)
        {
            var getById = await _consultationScheduleService.GetByStudent(studentId);
            return Ok(getById);
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddConsultationSchedule(AddConsultationScheduleRequest request)
        {
            var addConsultationSchedule = await _consultationScheduleService.AddConsultationScheduleAsync(request);
            return Ok(addConsultationSchedule);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(SearchConsultationScheduleRequest request)
        {
            var result = await _consultationScheduleService.SearchConsultationScheduleAsync(request);

            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateConsultationSchedule(UpdateConsultationScheduleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updateConsultationSchedule = await _consultationScheduleService.UpdateConsultationScheduleAsync(request);
            return Ok(updateConsultationSchedule);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsultationSchedule(int id)
        {
            var deleteConsultationSchedule = await _consultationScheduleService.DeleteConsultationScheduleAsync(id);
            return Ok();
        }
    }
}
