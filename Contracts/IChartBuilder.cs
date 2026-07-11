using AccountingApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace AccountingApp.Contracts
{
    public interface IChartBuilder
    {
        IChartBuilder SetTimeRange(DateTime start, DateTime end);
        IChartBuilder SetFilters(Dictionary<string, List<string>> filters);
        IChartBuilder SetGroups(List<string> groups);

        IChartBuilder GetData();

        IChartBuilder AnalyzeData();

        Chart Build();
    }
}
