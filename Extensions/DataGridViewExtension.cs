using AccountingApp.Attributes;
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
    internal static class DataGridViewExtension
    {
        public static void CreateComboxColumn(this DataGridView dataGridView, PropertyInfo prop)
        {
            DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn
            {
                Name = $"{prop.Name}_ComboBox",
                Tag = prop.Name,
                HeaderText = prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName?.ToString(),
                DataPropertyName = prop.Name,
            };
            dataGridView.Columns[prop.Name].Visible = false;
            dataGridView.Columns.Add(comboBoxColumn);

            if (prop.Name != "Details")
            {
                comboBoxColumn.DataSource = typeof(DataModel).GetField(prop.Name).GetValue(null);
            }
        }

        public static void CreateImageColumn(this DataGridView dataGridView, PropertyInfo prop)
        {
            dataGridView.Columns.Add(new DataGridViewImageColumn
            {
                Name = $"{prop.Name}_Img",
                HeaderText = prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName?.ToString(),
                ImageLayout = DataGridViewImageCellLayout.Zoom,
            });
            dataGridView.Columns[prop.Name].Visible = false;
        }

        public static void CreateTextBoxColumn(this DataGridView dataGridView, PropertyInfo prop)
        {
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = prop.Name,
                HeaderText = prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName?.ToString(),
                DataPropertyName = prop.Name
            });
            dataGridView.Columns[prop.Name].Visible = false;
        }
    }
}
