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
    public class PieChartBuilder : IChartBuilder
    {
        Chart _chart = new Chart();
        Dictionary<string, List<string>> _filters;
        List<string> _groups;
        DateTime _start;
        DateTime _end;
        List<AnalyzeDTO> _data = new List<AnalyzeDTO>();
        AnalyzeService _analyzeService = new AnalyzeService();

        public IChartBuilder AnalyzeData()
        {
            _chart.Titles.Add("帳務統計");
            //chart.BackColor = Color.Transparent;
            _chart.Legends.Add(new Legend()
            {
                Title = "分類",
                TitleBackColor = Color.Transparent,
                TitleForeColor = Color.Blue,
                TitleFont = new Font("微软雅黑", 10f, FontStyle.Regular),
                BackColor = Color.Transparent,
                Font = new Font("微软雅黑", 8f, FontStyle.Regular),
                ForeColor = Color.Blue,
                Position = new ElementPosition { Auto = true },
            });

            ChartArea outerArea = new ChartArea("OuterArea")
            {
                BackColor = Color.Transparent,
                Position = new ElementPosition(0, 10, 90, 90)
            };
            ChartArea innerArea = new ChartArea("InnerArea")
            {
                BackColor = Color.Transparent,
                Position = new ElementPosition(20, 30, 50, 50) // 與 outerArea 同心(中心對齊在 45,55)，半徑縮小
            };
            _chart.ChartAreas.Add(outerArea); // 外層先畫：大圓 + 外側原值
            _chart.ChartAreas.Add(innerArea); // 內層後畫：疊在外層上方，小圓 + 內側百分比

            List<string> keys = _data.Select(x => x.key).ToList();
            List<int> values = _data.Select(x => x.sum).ToList();
            Series outer = new Series()
            {
                Label = "#VAL",// 讓每個 legend 項目顯示對應的 key
                LegendText = "#AXISLABEL",
                ChartType = SeriesChartType.Pie,
                //XValueType = ChartValueType.String,
                //IsVisibleInLegend = true,
                //IsValueShownAsLabel = true,
                //ShadowOffset = 0,
                CustomProperties = "PieLabelStyle = Outside, PieLineColor = Gray",
                ChartArea = "OuterArea",
                BorderColor = Color.White
            };
            Series inner = new Series()
            {
                Label = "#PERCENT{P1}",// 讓每個 legend 項目顯示對應的 key
                LabelForeColor = Color.Black,
                //LegendText = "Value: #VALX \n Percentage: #PERCENT",// 讓每個 legend 項目顯示對應的 key
                ChartType = SeriesChartType.Pie,
                //XValueType = ChartValueType.String,
                IsVisibleInLegend = false,
                //IsValueShownAsLabel = true,
                //ShadowOffset = 0,
                CustomProperties = "PieLabelStyle = Inside",
                ChartArea = "InnerArea",
            };

            foreach (var item in _data)
            {
                // 加到外層
                DataPoint dpOuter = new DataPoint();
                dpOuter.SetValueY(item.sum);
                dpOuter.AxisLabel = item.key;
                outer.Points.Add(dpOuter);

                // 加到內層
                DataPoint dpInner = new DataPoint();
                dpInner.SetValueY(item.sum);
                inner.Points.Add(dpInner);
            }
            _chart.Series.Add(outer);
            _chart.Series.Add(inner);

            for (int i = 0; i < outer.Points.Count; i++)
            {
                // 強制讓內層的扇區顏色等於外層，避免預設分配跑掉
                inner.Points[i].Color = outer.Points[i].Color;
            }
            return this;
        }

        public IChartBuilder GetData()
        {
            _data = _analyzeService.AnalyzeData(_start, _end, _filters, _groups);
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
