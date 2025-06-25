using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smmsbe.Services;
using Smmsbe.Services.Helpers;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;
using Smmsbe.WebApi.Helpers;

namespace Smmsbe.WebApi.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/parent")]
    [ApiController]
    public class parentController : BaseAccountController
    {
        private readonly IParentService _parentService;

        public parentController(IParentService parentService)
        {
            _parentService = parentService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginParentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var acc = await _parentService.AuthorizeAsync(request.Email, request.Password);

            return Ok(new
            {
                Success = true,
                User = new
                {
                    Id = acc.ParentId,
                    acc.Email,
                    acc.FullName,
                    AccessToken = AccessTokenGenerator.GenerateExpiringAccessToken(DateTime.Now.ToVNTime().AddDays(1))
                }
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var getById = await _parentService.GetById(id);
            return Ok(getById);
        }

        [HttpGet("getStudents{studentId}")]
        public async Task<IActionResult> GetStudents(int studentId)
        {
            var result = await _parentService.GetParentFromStudent(studentId);
            return Ok(result);
        }

        /*[HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _parentService.GetAllAsync();
            return Ok(result);
        }*/

        [HttpPost("add")]
        public async Task<IActionResult> AddAccount([FromBody] AddParentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var account = await _parentService.AddParentAsync(request);

            return Ok(new
            {
                account.ParentId,
                account.FullName,
                account.PhoneNumber,
                account.Email
            });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAccount(UpdateParentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var acc = await _parentService.UpdateParentAsync(request);

            return Ok(acc);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(SearchParentRequest request)
        {
            var result = await _parentService.SearchParentAsync(request);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var result = await _parentService.DeleteParentAsync(id);

            return Ok();
        }

        [HttpPost("approve")]
        public async Task<IActionResult> Approve(int parentId)
        {
            var result = await _parentService.ApproveParentAsync(parentId);
            return Ok(new { success = result });
        }

        //Manager sẽ kích hoạt bằng tay
        /*[HttpPost("activate")]
        public async Task<IActionResult> Activate(ActivateRequest request)
        {
            var result = await _parentService.ActivateAccountAsync(request.Code);
            return Ok(new { success = result });
        }*/

        //Parent nhấn vào link để kích hoạt  
        /*[HttpGet("activate/{code}")]
        public async Task<IActionResult> ActivateParent(string code)
        {
            var result = await _parentService.ActivateAccountAsync(code);
            return Ok(new { success = result });
        }*/

        protected override async Task<bool> ActivateAccountAsync(string code)
        {
            return await _parentService.ActivateAccountAsync(code);
        }
    }
}
