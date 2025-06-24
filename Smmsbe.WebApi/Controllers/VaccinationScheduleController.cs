using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smmsbe.Services;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;

namespace Smmsbe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class vaccinationScheduleController : ControllerBase
    {
        private readonly IVaccinationScheduleService _vaccinationScheduleService;
        public vaccinationScheduleController(IVaccinationScheduleService vaccinationScheduleService)
        {
            _vaccinationScheduleService = vaccinationScheduleService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var getById = await _vaccinationScheduleService.GetById(id);
            return Ok(getById);
        }

        [HttpGet("getByForm{formId}")]
        public async Task<IActionResult> GetByFrom(int formId)
        {
            var getById = await _vaccinationScheduleService.GetByForm(formId);
            return Ok(getById);
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddVaccinationSchedule(AddVaccinationScheduleRequest request)
        {
            var addVaccinationSchedule = await _vaccinationScheduleService.AddVaccinationScheduleAsync(request);
            return Ok(addVaccinationSchedule);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(SearchVaccinationScheduleRequest request)
        {
            var result = await _vaccinationScheduleService.SearchVaccinationScheduleAsync(request);

            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateVaccinationSchedule(UpdateVaccinationScheduleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updateVaccinationSchedule = await _vaccinationScheduleService.UpdateVaccinationScheduleAsync(request);
            return Ok(updateVaccinationSchedule);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVaccinationSchedule(int id)
        {
            var deleteVaccinationSchedule = await _vaccinationScheduleService.DeleteVaccinationScheduleAsync(id);
            return Ok();
        }
    }
}
