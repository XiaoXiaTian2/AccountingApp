using AccountingApp.Attributes;
using AccountingApp.Enums;
using AccountingApp.Extensions;
using AccountingApp.Models;
using AccountingApp.Utility;
using CSV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace AccountingApp.Views
{
    public partial class 記帳本 : Form
    {
        private List<RecordModel> records;

        public 記帳本()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //datagridview.DataSource = list;
            //DataGridView 原理
            //DataGridViewColumn : 根據listModel的Property自動創建 DataGridViewTextboxColumn
            //DataGridViewRow: 根據list每一筆資料創建DataGridViewRow
            //DataGridViewCell: 根據每一筆資料的欄位，創建DataGridViewTextboxCell 進行欄位資料填充
            //dataGridView1.AutoGenerateColumns = false;

            this.DebounceClick<int>((num) =>
            {
                var rows = dataGridView1.Rows;
                for (int i = 0; i < rows.Count; i++)
                {
                    ((Bitmap)(rows[i].Cells["Img1_Img"].Value)).Dispose();
                    ((Bitmap)(rows[i].Cells["Img2_Img"].Value)).Dispose();
                }

                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                var diff = end_time.Value - start_time.Value;
                records = new List<RecordModel>();
                for (int i = 0; i <= diff.Days; i++)
                {
                    string basePath = AcoountingTargetHelper.GetAccountingFolder(start_time.Value.AddDays(i));
                    //var day = start_time.Value.AddDays(i).ToString("yyyyMMdd");
                    string filePath = $"{basePath}/data/data.csv";
                    if (File.Exists(filePath))
                    {
                        records.AddRange(CSVHelper.Read<RecordModel>(filePath));
                    }
                }
                dataGridView1.DataSource = records;

                this.ReRender();
            }, 2, 400);

            // HW: 各一張 1 MB / small_40 *40 
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var column = dataGridView1.Columns[e.ColumnIndex];
            string date = records[e.RowIndex].Date.Replace("-", "");

            string basePath = "E:/csharp_course/accounting";
            string filePath = $"{basePath}/{date}/data/data.csv";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                FileStream stream = File.Create(filePath);
                stream.Close();
            }

            string propertyName = column.Tag.ToString();

            var record = records[e.RowIndex];
            var prop = record.GetType().GetProperty(propertyName);
            if (prop != null && prop.CanWrite)
            {
                if (
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell comboBoxCell
                    && comboBoxCell.OwningColumn.Tag.ToString() == "Categories"
                )
                {
                    DataModel.Details.TryGetValue(comboBoxCell.Value.ToString(), out List<string> details);

                    var detail = (DataGridViewComboBoxCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1];
                    detail.DataSource = details;
                    detail.Value = details[0];
                }

                var value = dataGridView1
                            .Rows[e.RowIndex]
                            .Cells[e.ColumnIndex]
                            .Value?.ToString();

                prop.SetValue(record, value);
            }

            CSVHelper.WriteList(filePath, records);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            // 刪除按鈕、圖片雙擊放大
            if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewImageColumn)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Tag?.ToString() == "刪除")
                {
                    var column = dataGridView1.Columns[e.ColumnIndex];
                    string date = records[e.RowIndex].Date.Replace("-", "");

                    string basePath = "E:/csharp_course/accounting";
                    string filePath = $"{basePath}/{date}/data/data.csv";
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        FileStream stream = File.Create(filePath);
                        stream.Close();

                        dataGridView1.Rows[e.RowIndex].Cells
                           .OfType<DataGridViewImageCell>().ToList()
                           .ForEach(cel =>
                           {
                               ((Bitmap)(cel.Value)).Dispose();
                               var img1 = records[e.RowIndex].Img1;
                               var img2 = records[e.RowIndex].Img2;
                               File.Delete(img1);
                               File.Delete(img2);
                           });
                    }

                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    records.RemoveAt(e.RowIndex);
                    dataGridView1.DataSource = records;
                    CSVHelper.WriteList(filePath, records);
                    this.ReRender();

                    return;
                }

                var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value is Image)
                {
                    this.Width = this.Height = 800;
                    this.StartPosition = FormStartPosition.CenterScreen;

                    string path = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex - 8].Value.ToString().Replace("small_", "");
                    var picForm = new ImageForm(path);
                    picForm.ShowDialog();
                }
            }
        }

        private void ReRender()
        {
            foreach (var prop in typeof(RecordModel).GetProperties())
            {
                // 擴充方法
                if (prop.GetCustomAttributes().Any((x) => x.GetType() == typeof(ComboBoxColumnAttribute)))
                {
                    DataGridViewExtension.CreateComboxColumn(dataGridView1, prop);
                    continue;
                }
                if (prop.GetCustomAttributes().Any((x) => x.GetType() == typeof(ImageColumnAttribute)))
                {
                    DataGridViewExtension.CreateImageColumn(dataGridView1, prop);
                    continue;
                }
                DataGridViewExtension.CreateTextBoxColumn(dataGridView1, prop);
            }

            var rows = dataGridView1.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                DataModel.Details.TryGetValue(rows[i].Cells["Categories_ComboBox"].Value.ToString(), out List<string> detail);
                DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)rows[i].Cells["Details_ComboBox"];
                comboBoxCell.DataSource = detail;
                comboBoxCell.Value = detail[0];

                rows[i].Cells["Img1_Img"].Value = ImageSet(rows[i].Cells["Img1"].Value?.ToString());
                rows[i].Cells["Img2_Img"].Value = ImageSet(rows[i].Cells["Img2"].Value?.ToString());
                ////((Bitmap)(rows[i].Cells["Img1_Img"].Value)).Dispose();
                ////((Bitmap)(rows[i].Cells["Img2_Img"].Value)).Dispose();
            }

            string imgPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Image",
                "delete.png"
            );

            Image img;
            using (var bmpTemp = new Bitmap(imgPath))
            {
                img = new Bitmap(bmpTemp);
            }
            var deleteColumn = new DataGridViewImageColumn
            {
                Name = "刪除",
                Tag = "刪除",
                HeaderText = "刪除",
                Image = img,
                ImageLayout = DataGridViewImageCellLayout.Zoom,
            };

            dataGridView1.Columns.Add(deleteColumn);

            GC.Collect();
        }

        private void CommitData(object sender, EventArgs e)
        {
            var dgw = (DataGridView)sender;
            if (dgw.CurrentCell is DataGridViewComboBoxCell)
            {
                dgw.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private Bitmap ImageSet(string path)
        {
            return new Bitmap(new MemoryStream(File.ReadAllBytes(path)));
        }
    }
}
