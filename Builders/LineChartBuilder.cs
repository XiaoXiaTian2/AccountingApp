using AccountingApp.Contracts;
using AccountingApp.Enums;
using AccountingApp.Models;
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
    public class LineChartBuilder : IChartBuilder
    {
        private static readonly string[] seriesNames = { "本週", "上週" };
        private static readonly string[] weekdayNames = { "週日", "週一", "週二", "週三", "週四", "週五", "週六" };
        Chart _chart = new Chart();
        Dictionary<string, List<string>> _filters;
        List<string> _groups;
        DateTime _start;
        DateTime _end;
        List<List<AnalyzeLineChartModel>> _series = new List<List<AnalyzeLineChartModel>>();
        AnalyzeService _analyzeService = new AnalyzeService();

        string _period = null;
        public LineChartBuilder(string period) { _period = period; }

        public IChartBuilder AnalyzeData()
        {
            _chart.Titles.Add("折線圖");
            _chart.BackColor = Color.Transparent;
            _chart.ChartAreas.Add(new ChartArea());
            _chart.Legends.Add(new Legend());

            for (int i = 0; i < _series.Count; i++)
            {
                List<AnalyzeLineChartModel> line = _series[i];
                Series s = new Series()
                {
                    Name = i < seriesNames.Length ? seriesNames[i] : (i + 1).ToString(),
                    Label = "#VAL",
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 3,
                    IsValueShownAsLabel = true,
                    ShadowOffset = 0,
                    XValueType = ChartValueType.String,
                };

                for (int j = 0; j < line.Count; j++)
                {
                    s.Points.AddXY(weekdayNames[(int)line[j].date.DayOfWeek], line[j].sum);
                }

                _chart.Series.Add(s);
            }

            return this;
        }

        public IChartBuilder GetData()
        {
            _series.Add(_analyzeService.AnalyzeChartData(this._start, this._end, this._filters, this._groups));
            int periodDiff = (_period == Period.每週.ToString()) ? -7 : -30;
            DateTime previousWeek = this._start.AddDays(periodDiff);
            _series.Add(_analyzeService.AnalyzeChartData(previousWeek, this._start.AddDays(-1), this._filters, this._groups));
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
