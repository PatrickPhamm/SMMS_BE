using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Smmsbe.Repositories.Interfaces;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services
{
    public class ReportService : IReportService
    {
        private readonly IParentRepository _parentRepository;
        private readonly INurseRepository _nurseRepository;
        private readonly IStudentRepository _studentRepository;

        public ReportService(IParentRepository parentRepository,
            INurseRepository nurseRepository,
            IStudentRepository studentRepository)
        {
            _parentRepository = parentRepository;
            _studentRepository = studentRepository;
            _nurseRepository = nurseRepository;
        }

        public async Task<SummaryReportResponse> GetSummaryReport()
        {
            //Tổng bố mẹ
            var totalParents = await _parentRepository.GetAll().CountAsync();

            //Tổng y tá
            var totalNurses = await _nurseRepository.GetAll().CountAsync();

            //Tổng học sinh
            var totalStudents = await _studentRepository.GetAll().CountAsync();

            return new SummaryReportResponse
            {
                TotalParents = totalParents,
                TotalNurses = totalNurses,
                TotalStudents = totalStudents
            };
        }
    }
}
