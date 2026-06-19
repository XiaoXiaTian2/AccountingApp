using AccountingApp.Extensions;
using AccountingApp.Models;
using AccountingApp.Models.DTOs;
using AccountingApp.Presenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AccountingApp.Contracts.AnalyzeContract;

namespace AccountingApp.Views
{
    public partial class 帳戶分析 : Form, IAnalyzeView
    {
        private IAnalyzePresenter _presenter;
        List<string> groups = new List<string>(); // 類型/支付對象/付款方式
        Dictionary<string, List<string>> filters = new Dictionary<string, List<string>>();
        // 飲食: 午餐/晚餐
        // 交通: 捷運/公車
        public 帳戶分析()
        {
            InitializeComponent();
            flowLayoutPanel1.CreateGroupFlowLayoutPanel(GroupsCheckChanged);
            flowLayoutPanel2.CreateFilterFlowLayoutPanel(FiltersCheckChanged);
            _presenter = new AnalyzePresenter(this);
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

        public bool OnAnalyzeResponse(List<AnalyzeDTO> data)
        {
            dataGridView1.DataSource = data;

            return true;
        }

        private void SendRequest(object sender, EventArgs e)
        {
            //_presenter.SendAnalyzeRequest(start_time.Value, end_time.Value, filters, groups);
        }
    }
}
