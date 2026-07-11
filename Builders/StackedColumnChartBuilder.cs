using AccountingApp.Contracts;
using AccountingApp.Enums;
using AccountingApp.Models;
using AccountingApp.Models.DTOs;
using AccountingApp.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace AccountingApp.Builders
{
    public class StackedColumnChartBuilder : IChartBuilder
    {
        Chart _chart = new Chart();
        Dictionary<string, List<string>> _filters;
        List<string> _groups;
        DateTime _start;
        DateTime _end;
        Dictionary<string, Dictionary<string, string>> _data = new Dictionary<string, Dictionary<string, string>>();
        AnalyzeService _analyzeService = new AnalyzeService();

        public IChartBuilder AnalyzeData()
        {
            _chart.Titles.Add("疊圖");
            _chart.BackColor = Color.Transparent;
            _chart.ChartAreas.Add(new ChartArea());
            _chart.Legends.Add(new Legend());

            // X 軸：日期（排序）；每個群組各一條 series，所有 series 長度一致才能正確堆疊
            List<string> dates = _data.Keys.OrderBy(date => date).ToList();
            List<string> groupNames = _data.SelectMany(x => x.Value.Keys).Distinct().ToList();

            foreach (string groupName in groupNames)
            {
                List<string> yValues = dates
                    .Select(date => _data[date].TryGetValue(groupName, out string money) ? money : "0")
                    .ToList();

                Series s = new Series(groupName)
                {
                    Label = "#VAL",
                    ChartType = SeriesChartType.StackedColumn,
                    IsValueShownAsLabel = true,
                    ShadowOffset = 0,
                    XValueType = ChartValueType.String,
                    LegendText = groupName,
                };

                s.Points.DataBindXY(dates, yValues);

                _chart.Series.Add(s);
            }

            return this;
        }

        public IChartBuilder GetData()
        {
            _data = _analyzeService.AnalyzeColumnStackData(_start, _end, _filters, _groups);
            return this;
        }

        public IChartBuilder SetFilters(Dictionary<string, List<string>> filters)
        {
            _filters = filters;
            return this;
        }

        public IChartBuilder SetGroups(List<string> groups)
        {
            _groups = groups;
            return this;
        }

        public IChartBuilder SetTimeRange(DateTime start, DateTime end)
        {
            _start = start;
            _end = end;
            return this;
        }

        public Chart Build()
        {
            return _chart;
        }
    }
}
