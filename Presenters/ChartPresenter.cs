using AccountingApp.Builders;
using AccountingApp.Contracts;
using AccountingApp.Enums;
using AccountingApp.Models;
using AccountingApp.Models.DAOs;
using AccountingApp.Models.DTOs;
using AccountingApp.Models.Repositories;
using AccountingApp.Services;
using AccountingApp.Utility;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static AccountingApp.Contracts.AddRecordContract;
using static AccountingApp.Contracts.ChartContract;

namespace AccountingApp.Presenters
{
    internal class ChartPresenter : IChartPresenter
    {
        IChartView _view = null;

        public ChartPresenter(IChartView view)
        {
            _view = view;
        }

        public void GetChartDataDTO(DateTime start, DateTime end, Dictionary<string, List<string>> filters, List<string> groups, string period, ChartType chartType)
        {
            Type classType = Type.GetType($"AccountingApp.Builders.{chartType}ChartBuilder");

            IChartBuilder builder = chartType == ChartType.Line
                    ? (IChartBuilder)Activator.CreateInstance(classType, new object[] { period })
                    : (IChartBuilder)Activator.CreateInstance(classType);

            Chart chart = builder.SetTimeRange(start, end).SetFilters(filters).SetGroups(groups).GetData().AnalyzeData().Build();

            _view.OnGetChartDataDTO(chart);
        }
    }
}
