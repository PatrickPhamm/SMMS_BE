using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smmsbe.Services;
using Smmsbe.Services.Helpers;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;
using Smmsbe.WebApi.Helpers;

namespace Smmsbe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class studentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public studentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginStudentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var acc = await _studentService.AuthorizeAsync(request.StudentNumber, request.Password);

            return Ok(new
            {
                Success = true,
                User = new
                {
                    Id = acc.StudentId,
                    acc.FullName, 
                    acc.ClassName,
                    acc.StudentNumber,
                    AccessToken = AccessTokenGenerator.GenerateExpiringAccessToken(DateTime.Now.ToVNTime().AddDays(1))
                }
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            //var getById = await _studentService.GetById(id);
            var getById = await _studentService.GetByIdAsync(id);
            return Ok(getById);
        }

        [HttpGet("getParent{parentId}")]
        public async Task<IActionResult> GetResultsBySchedule(int parentId)
        {
            var getId = await _studentService.GetStudentByParent(parentId);
            return Ok(getId);
        }

        /*[HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _studentService.GetAllAsync();
            return Ok(result);
        }*/

        [HttpPost("add")]
        public async Task<IActionResult> AddAccount([FromBody] AddStudentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var account = await _studentService.AddStudentAsync(request);

            return Ok(new
            {
                account.StudentId,
                account.ParentId,
                account.FullName,
                account.DateOfBirth,
                account.Gender,
                account.StudentNumber
            });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAccount(UpdateStudentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var acc = await _studentService.UpdateStudentAsync(request);

            return Ok(acc);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(SearchStudentRequest request)
        {
            var result = await _studentService.SearchStudentAsync(request);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var result = await _studentService.DeleteStudentAsync(id);

            return Ok();
        }
    }
}
