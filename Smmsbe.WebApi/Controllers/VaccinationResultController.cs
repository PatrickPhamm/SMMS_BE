using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smmsbe.Services;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;

namespace Smmsbe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class vaccinationResultController : ControllerBase
    {
        private readonly IVaccinationResultService _vaccinationResultService;
        public vaccinationResultController(IVaccinationResultService vaccinationResultService)
        {
            _vaccinationResultService = vaccinationResultService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var getById = await _vaccinationResultService.GetById(id);
            return Ok(getById);
        }

        [HttpGet("getResultsBySchedule{scheduleId}")]
        public async Task<IActionResult> GetResultsBySchedule(int scheduleId)
        {
            var getId = await _vaccinationResultService.GetResultsBySchedule(scheduleId);
            return Ok(getId);
        }

        [HttpGet("getResultsByProfile{profileId}")]
        public async Task<IActionResult> GetResultsByProfile(int profileId)
        {
            var getId = await _vaccinationResultService.GetResultsByHealthProfile(profileId);
            return Ok(getId);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddVaccinationResult(AddVaccinationResultRequest request)
        {
            var addVaccinationResult = await _vaccinationResultService.AddVaccinationResultAsync(request);
            return Ok(addVaccinationResult);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(SearchVaccinationResultRequest request)
        {
            var result = await _vaccinationResultService.SearchVaccinationResultAsync(request);

            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateVaccinationResult(UpdateVaccinationResultRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updateVaccinationResult = await _vaccinationResultService.UpdateVaccinationResultAsync(request);
            return Ok(updateVaccinationResult);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVaccinationResult(int id)
        {
            var deleteVaccinationResult = await _vaccinationResultService.DeleteVaccinationResultAsync(id);
            return Ok();
        }

        [HttpPost("complete/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _vaccinationResultService.CompleteVaccinationResultAsync(id);

            return Ok(result);
        }
    }
}
