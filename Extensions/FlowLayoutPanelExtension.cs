using AccountingApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountingApp.Extensions
{
    internal static class FlowLayoutPanelExtension
    {
        private static FlowLayoutPanel flow1;
        private static FlowLayoutPanel flow2;
        public static void CreateGroupFlowLayoutPanel(this FlowLayoutPanel flow, EventHandler eventHandler)
        {
            flow1 = flow;

            Dictionary<string, string> groupconditions = new Dictionary<string, string>()
            {
                {"類型", "Categories" },
                {"對象", "Members" },
                {"付款方式", "Payways" },
            };
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel()
            {
                Width = flow.Width,
                Height = 25,
            };
            foreach (var item in groupconditions) // 頂層
            {
                CheckBox checkBox = new CheckBox() { Text = item.Key, Width = 80, Tag = item.Value };
                checkBox.CheckedChanged += ToggleVisbility;
                if (item.Value != "Categories")
                    checkBox.CheckedChanged += eventHandler;
                flowLayoutPanel.Controls.Add(checkBox);
            }
            flow.Controls.Add(flowLayoutPanel);

            // subcategory
            FlowLayoutPanel CateFlowLayoutPanels = new FlowLayoutPanel()
            {
                Width = flow.Width,
                Height = 30,
                Visible = false,
                Tag = "Categories"
            };
            foreach (var item in DataModel.Categories)
            {
                CheckBox checkBox = new CheckBox() { Text = item, Width = 80, Tag = item };
                checkBox.CheckedChanged += ToggleDetailVisibility;
                checkBox.CheckedChanged += eventHandler;
                CateFlowLayoutPanels.Controls.Add(checkBox);
            }
            flow.Controls.Add(CateFlowLayoutPanels);

            // member group
            FlowLayoutPanel MemberflowLayoutPanels = new FlowLayoutPanel()
            {
                Width = flow.Width,
                Height = 30,
                Visible = false,
                Tag = "Members"
            };
            foreach (var item in DataModel.Members)
            {
                CheckBox checkBox = new CheckBox() { Text = item, Width = 80 };
                checkBox.CheckedChanged += eventHandler;
                MemberflowLayoutPanels.Controls.Add(checkBox);
            }
            flow.Controls.Add(MemberflowLayoutPanels);

            // payway group
            FlowLayoutPanel PaywayflowLayoutPanels = new FlowLayoutPanel()
            {
                Width = flow.Width,
                Height = 30,
                Visible = false,
                Tag = "Payways"
            };
            foreach (var item in DataModel.Payways)
            {
                CheckBox checkBox = new CheckBox() { Text = item, Width = 75 };
                checkBox.CheckedChanged += eventHandler;
                PaywayflowLayoutPanels.Controls.Add(checkBox);
            }
            flow.Controls.Add(PaywayflowLayoutPanels);
        }

        public static void CreateFilterFlowLayoutPanel(this FlowLayoutPanel flow, EventHandler eventHandler)
        {
            flow2 = flow;
            foreach (var item in DataModel.Details)
            {
                FlowLayoutPanel innerPanel = new FlowLayoutPanel()
                {
                    Width = flow.Width,
                    Height = 30,
                    Tag = item.Key,
                    Visible = false
                };
                CheckBox allCheck = new CheckBox() { Text = "全選", Width = 75 };
                allCheck.CheckedChanged += AllCheck;
                innerPanel.Controls.Add(allCheck);
                foreach (var item1 in item.Value)
                {
                    CheckBox checkBox = new CheckBox() { Text = item1, Width = 75, Tag = item.Key };
                    checkBox.CheckedChanged += CheckBox_CheckedChanged;
                    checkBox.CheckedChanged += eventHandler;
                    innerPanel.Controls.Add(checkBox);
                }
                flow.Controls.Add(innerPanel);
            }
            FlowLayoutPanel MemberflowLayoutPanelFilters = new FlowLayoutPanel()
            {
                Width = flow.Width,
                Height = 30,
                Visible = false,
                Tag = "Members"
            };
            CheckBox memberAllCheck = new CheckBox() { Text = "全選", Width = 75 };
            memberAllCheck.CheckedChanged += AllCheck;
            MemberflowLayoutPanelFilters.Controls.Add(memberAllCheck);
            foreach (var item in DataModel.Members)
            {
                CheckBox checkBox = new CheckBox() { Text = item, Width = 75 };
                checkBox.CheckedChanged += eventHandler;
                MemberflowLayoutPanelFilters.Controls.Add(checkBox);
            }
            FlowLayoutPanel PaywayflowLayoutPanelFilters = new FlowLayoutPanel()
            {
                Width = flow.Width,
                Height = 30,
                Visible = false,
                Tag = "Payways"
            };
            CheckBox paywayAllCheck = new CheckBox() { Text = "全選", Width = 75 };
            paywayAllCheck.CheckedChanged += AllCheck;
            PaywayflowLayoutPanelFilters.Controls.Add(paywayAllCheck);
            foreach (var item in DataModel.Payways)
            {
                CheckBox checkBox = new CheckBox() { Text = item, Width = 75 };
                checkBox.CheckedChanged += eventHandler;
                PaywayflowLayoutPanelFilters.Controls.Add(checkBox);
            }

            flow.Controls.Add(MemberflowLayoutPanelFilters);
            flow.Controls.Add(PaywayflowLayoutPanelFilters);
        }

        private static void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox current = (CheckBox)sender;
            bool allChecked = current.Parent.Controls.OfType<CheckBox>().Where(x => x.Text != "全選").All(x => x.Checked);
            CheckBox allCheck = current.Parent.Controls.OfType<CheckBox>().FirstOrDefault(x => x.Text == "全選");
            allCheck.Tag = false;
            allCheck.Checked = allChecked;
        }

        private static void AllCheck(object sender, EventArgs e)
        {
            CheckBox allCheck = (CheckBox)sender;

            foreach (CheckBox item in allCheck.Parent.Controls.OfType<CheckBox>()
                .Where(x => x != allCheck))
            {
                item.CheckedChanged -= CheckBox_CheckedChanged;

                item.Checked = allCheck.Checked;

                item.CheckedChanged += CheckBox_CheckedChanged;
            }
        }

        private static void ToggleVisbility(object sender, EventArgs e)
        {
            CheckBox visbility = (CheckBox)sender;
            foreach (FlowLayoutPanel flow in flow1.Controls.OfType<FlowLayoutPanel>())
            {
                if (Equals(flow.Tag, visbility.Tag) && visbility.Tag?.ToString() == "Categories")
                {
                    flow.Visible = visbility.Checked;
                    var checkedL = flow.Controls.OfType<CheckBox>().ToList();
                    foreach (var item1 in checkedL)
                    {
                        item1.Checked = false;
                    }
                }
            }
            // 篩選條件
            foreach (FlowLayoutPanel flow in flow2.Controls.OfType<FlowLayoutPanel>())
            {
                if (Equals(flow.Tag, visbility.Tag))
                {
                    flow.Visible = visbility.Checked;
                    var checkedL = flow.Controls.OfType<CheckBox>().ToList();
                    foreach (var item1 in checkedL)
                    {
                        item1.Checked = false;
                    }
                }
            }
        }

        private static void ToggleDetailVisibility(object sender, EventArgs e)
        {
            CheckBox detailVisibility = (CheckBox)sender;
            FlowLayoutPanel flow = flow2.Controls.OfType<FlowLayoutPanel>().FirstOrDefault();
            if (!Equals(flow.Tag, detailVisibility.Tag))
                return;

            flow.Visible = detailVisibility.Checked;
            if (!detailVisibility.Checked)
            {
                flow.Controls.OfType<CheckBox>().ToList().ForEach(item => item.Checked = false);
            }
        }
    }
}
