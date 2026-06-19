using AccountingApp.Models;
using AccountingApp.Models.DAOs;
using AccountingApp.Models.DTOs;
using AccountingApp.Models.Repositories;
using AccountingApp.Services;
using AccountingApp.Utility;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AccountingApp.Contracts.AddRecordContract;
using static AccountingApp.Contracts.AnalyzeContract;

namespace AccountingApp.Presenters
{
    internal class AnalyzePresenter : IAnalyzePresenter
    {
        IAnalyzeView _view = null;
        AnalyzeService analyzeService = new AnalyzeService();

        public AnalyzePresenter(IAnalyzeView view)
        {
            _view = view;
        }

        void IAnalyzePresenter.SendAnalyzeRequest(DateTime start, DateTime end, Dictionary<string, List<string>> filters, List<string> groups, string period)
        {
            List<AnalyzeDTO> analyzeDTOs = analyzeService.AnalyzeData(start, end, filters, groups);

            _view.OnAnalyzeResponse(analyzeDTOs);
        }
    }
}
