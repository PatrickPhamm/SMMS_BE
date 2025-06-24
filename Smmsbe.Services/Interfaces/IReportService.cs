using Smmsbe.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Interfaces
{
    public interface IReportService
    {
        Task<SummaryReportResponse> GetSummaryReport();
    }
}
