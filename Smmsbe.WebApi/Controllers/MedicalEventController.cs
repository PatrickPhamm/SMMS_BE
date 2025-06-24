using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smmsbe.Services;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;

namespace Smmsbe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class medicalEventController : ControllerBase
    {
        private readonly IMedicalEventService _medicalEventService;
        public medicalEventController(IMedicalEventService medicalEventService)
        {
            _medicalEventService = medicalEventService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var getById = await _medicalEventService.GetById(id);
            return Ok(getById);
        }

        [HttpGet("getMedicalByStudent")]
        public async Task<IActionResult> GetByStudent(int studentId)
        {
            var getById = await _medicalEventService.GetMedicalByStudent(studentId);
            return Ok(getById);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddMedicalEvent(AddMedicalEventRequest request)
        {
            var addMedicalEvent = await _medicalEventService.AddMedicalEventAsync(request);
            return Ok(addMedicalEvent);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(SearchMedicalEventRequest request)
        {
            var result = await _medicalEventService.SearchMedicalEventAsync(request);

            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateMedicalEvent(UpdateMedicalEventRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updateMedicalEvent = await _medicalEventService.UpdateMedicalEventAsync(request);
            return Ok(updateMedicalEvent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalEvent(int id)
        {
            var deleteMedicalEvent = await _medicalEventService.DeleteMedicalEventAsync(id);
            return Ok();
        }
    }
}
