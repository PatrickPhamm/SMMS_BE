using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smmsbe.Services;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;

namespace Smmsbe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class formController : ControllerBase
    {
        private readonly IFormService _formService;

        public formController(IFormService formService)
        {
            _formService = formService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _formService.GetById(id);

            return Ok(entity);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddForm([FromBody] AddFormRequest request)
        {
            try
            {
                var entity = await _formService.AddFormAsync(request);
                return Ok(entity);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the form.");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateForm(UpdateFormRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await _formService.UpdateFormAsync(request);

            return Ok(entity);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(SearchFormRequest request)
        {
            var result = await _formService.SearchFormAsync(request);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForm(int id)
        {
            var result = await _formService.DeleteFormAsync(id);

            return Ok();
        }
    }
}
