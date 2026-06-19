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
        private static readonly string[] weekdayNames = { "週日", "週一", "週二", "週三", "週四", "週五", "週六" };
        private static readonly string[] seriesNames = { "本週", "上週" };

        IChartView _view = null;
        AnalyzeService analyzeService = new AnalyzeService();

        public ChartPresenter(IChartView view)
        {
            _view = view;
        }

        public void GetChartDataDTO(DateTime start, DateTime end, Dictionary<string, List<string>> filters, List<string> groups, string period)
        {
            Chart chart = null;
            if (period == Period.每週.ToString())
            {
                // 8-15
                List<AnalyzeLineChartModel> series1 = analyzeService.AnalyzeChartData(start, end, filters, groups);
                // 1-7
                DateTime previousWeek = start.AddDays(-7);
                List<AnalyzeLineChartModel> series2 = analyzeService.AnalyzeChartData(previousWeek, start.AddDays(-1), filters, groups);

                //chart = GenerateLineChart(period, series1, series2);
                Dictionary<string, Dictionary<string, string>> columnStackData = analyzeService.AnalyzeColumnStackData(start, end, filters, groups);
                chart = GenerateColumnStackChart(columnStackData);

                List<AnalyzeDTO> data = analyzeService.AnalyzeData(start, end, filters, groups);
                //chart = GeneratePieChart(data);
            }

            _view.OnGetChartDataDTO(chart);
        }

        private Chart GeneratePieChart(List<AnalyzeDTO> data)
        {
            Chart chart = new Chart();
            chart.Titles.Add("帳務統計");
            //chart.BackColor = Color.Transparent;
            chart.Legends.Add(new Legend()
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
            chart.ChartAreas.Add(outerArea); // 外層先畫：大圓 + 外側原值
            chart.ChartAreas.Add(innerArea); // 內層後畫：疊在外層上方，小圓 + 內側百分比

            List<string> keys = data.Select(x => x.key).ToList();
            List<int> values = data.Select(x => x.sum).ToList();
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

            foreach (var item in data)
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
            chart.Series.Add(outer);
            chart.Series.Add(inner);

            //chart.Series[0].Points.DataBindXY(keys, values);
            //chart.Series[1].Points.DataBindXY(keys, values);
            //chart.ApplyPaletteColors();
            for (int i = 0; i < outer.Points.Count; i++)
            {
                // 強制讓內層的扇區顏色等於外層，避免預設分配跑掉
                inner.Points[i].Color = outer.Points[i].Color;
            }
            return chart;
        }

        private Chart GenerateLineChart(string period, params List<AnalyzeLineChartModel>[] series)
        {
            Chart chart = new Chart();
            chart.Titles.Add("折線圖");
            chart.BackColor = Color.Transparent;
            chart.ChartAreas.Add(new ChartArea());
            chart.Legends.Add(new Legend());

            for (int i = 0; i < series.Length; i++)
            {
                List<AnalyzeLineChartModel> line = series[i];
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

                chart.Series.Add(s);
            }

            return chart;
        }

        private Chart GenerateColumnStackChart(Dictionary<string, Dictionary<string, string>> data)
        {
            Chart chart = new Chart();
            chart.Titles.Add("疊圖");
            chart.BackColor = Color.Transparent;
            chart.ChartAreas.Add(new ChartArea());
            chart.Legends.Add(new Legend());

            var xValues = data.Select(x => x.Key).ToList();
            foreach (var item in data.OrderBy(x =>x.Key))
            {
                string groupName = item.Key;
                List<string> yValues = item.Value.Select(x => x.Value).ToList();
                //AnalyzeLineChartModel line = data[i];
                Series s = new Series()
                {
                    //Name = i < seriesNames.Length ? seriesNames[i] : (i + 1).ToString(),
                    Label = "#VAL",
                    ChartType = SeriesChartType.StackedColumn,
                    IsValueShownAsLabel = true,
                    ShadowOffset = 0,
                    XValueType = ChartValueType.String,
                };

                s.Points.DataBindXY(xValues, yValues);

                chart.Series.Add(s);
            }

            return chart;
        }
    }
}
