using AccountingApp.Models.DTOs;
using AutoMapper.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingApp.Contracts
{
    internal class AnalyzeContract
    {
        public interface IAnalyzePresenter
        {
            void SendAnalyzeRequest(DateTime start, DateTime end, Dictionary<string, List<string>> filters, List<string> groups, string period);
        }

        public interface IAnalyzeView
        {
            bool OnAnalyzeResponse(List<AnalyzeDTO> data);
        }
    }
}
