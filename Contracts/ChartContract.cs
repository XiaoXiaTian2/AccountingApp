using AccountingApp.Enums;
using AccountingApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace AccountingApp.Contracts
{
    internal class ChartContract
    {
        public interface IChartPresenter
        {
            void GetChartDataDTO(DateTime start, DateTime end, Dictionary<string, List<string>> filters, List<string> groups, string period, ChartType chartType);
        }

        public interface IChartView
        {
            void OnGetChartDataDTO(Chart chart);
        }
    }
}
