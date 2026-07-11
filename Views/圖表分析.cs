using AccountingApp.Enums;
using AccountingApp.Extensions;
using AccountingApp.Models.DTOs;
using AccountingApp.Presenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static AccountingApp.Contracts.AnalyzeContract;
using static AccountingApp.Contracts.ChartContract;

namespace AccountingApp.Views
{
    public partial class 圖表分析 : Form, IChartView
    {
        IChartPresenter _presenter = null;
        List<string> groups = new List<string>(); // 類型/支付對象/付款方式
        Dictionary<string, List<string>> filters = new Dictionary<string, List<string>>();
        private static Chart _chart = new Chart();

        public 圖表分析()
        {
            InitializeComponent();
            flowLayoutPanel1.CreateGroupFlowLayoutPanel(GroupsCheckChanged);
            flowLayoutPanel2.CreateFilterFlowLayoutPanel(FiltersCheckChanged);
            _presenter = new ChartPresenter(this);
            period.DataSource = Enum.GetNames(typeof(Enums.Period));
            period.SelectedIndex = 0;
            chartType.DataSource = Enum.GetNames(typeof(Enums.ChartType));
            chartType.SelectedIndex = 0;
        }

        private void GroupsCheckChanged(object sender, EventArgs e)
        {
            CheckBox chx = (CheckBox)sender;
            if (chx.Tag == null) // for category 頂層
                return;
            if (chx.Checked)
            {
                groups.Add(chx.Tag.ToString());
            }
            else
            {
                groups.Remove(chx.Tag.ToString());
            }
        }

        private void FiltersCheckChanged(object sender, EventArgs e)
        {
            CheckBox chx = (CheckBox)sender;
            if (chx.Text == "全選")
                return;
            string parentKey = chx.Parent.Tag?.ToString();
            bool hasKey = filters.TryGetValue(parentKey, out List<string> items);
            if (!hasKey)
            {
                if (chx.Checked)
                {
                    filters.Add(parentKey, new List<string> { chx.Text });
                }
            }
            else
            {
                if (chx.Checked)
                {
                    items.Add(chx.Text);
                }
                else
                {
                    items.Remove(chx.Text);
                    if (items.Count == 0)
                        filters.Remove(parentKey);
                }
            }
            filters.TryGetValue(chx.Parent.Tag?.ToString(), out List<string> itemms);
        }

        private void SendRequest(object sender, EventArgs e)
        {
            _presenter.GetChartDataDTO(start_time.Value, end_time.Value, filters, groups, period.SelectedValue.ToString(), (ChartType)Enum.Parse(typeof(ChartType), chartType.SelectedValue.ToString()));
        }

        // 6/1 6/2..
        public void OnGetChartDataDTO(Chart chart)
        {
            // 先清掉並釋放上一次的圖，避免每次查詢一直往下疊
            while (flowLayoutPanel3.Controls.Count > 0)
                flowLayoutPanel3.Controls[0].Dispose();
            flowLayoutPanel3.Controls.Add(chart);
        }

        private void ChangePeriod(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedValue.ToString() == ChartType.Line.ToString())
            {
                period.Visible = true;
                return;
            }
            period.Visible = false;
        }
    }
}
