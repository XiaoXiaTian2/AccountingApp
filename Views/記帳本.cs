using AccountingApp.Enums;
using AccountingApp.Models;
using CSV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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

        private string filePath = "E:/csharp_course/accounting/data/data.csv";
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
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = records = CSVHelper.Read<RecordModel>(filePath);

            this.ReRender();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 11)
            {
                DataModel.Details.TryGetValue(dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString(), out List<string> details);

                var detail = (DataGridViewComboBoxCell)dataGridView1.Rows[e.RowIndex].Cells[12];
                detail.DataSource = details;
                detail.Value = details[0];
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                FileStream stream = File.Create(filePath);
                stream.Close();

            }

            switch (e.ColumnIndex)
            {
                case 8:
                    records[e.RowIndex].Date = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    break;
                case 9:
                    records[e.RowIndex].Money = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    break;
                case 10:
                    records[e.RowIndex].Member = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    break;
                case 11:
                    records[e.RowIndex].Category = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    break;
                case 12:
                    records[e.RowIndex].Detail = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    break;
                case 13:
                    records[e.RowIndex].Payway = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    break;
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
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        FileStream stream = File.Create(filePath);
                        stream.Close();
                        ((Bitmap)(dataGridView1.Rows[e.RowIndex].Cells[14].Value)).Dispose();
                        ((Bitmap)(dataGridView1.Rows[e.RowIndex].Cells[15].Value)).Dispose();
                        var img1 = records[e.RowIndex].Img1;
                        var img2 = records[e.RowIndex].Img2;
                        File.Delete(img1);
                        File.Delete(img2);
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

                    var picForm = new ImageForm(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex - 8].Value.ToString());
                    picForm.Show();
                }
            }
        }

        private void ReRender()
        {
            foreach (var prop in typeof(RecordModel).GetProperties())
            {
                if (prop.Name == "Img1" || prop.Name == "Img2")
                {
                    var imgCol = new DataGridViewImageColumn
                    {
                        Name = prop.Name,
                        HeaderText = prop.Name,
                        ImageLayout = DataGridViewImageCellLayout.Zoom,
                    };

                    dataGridView1.Columns.Add(imgCol);
                }
                else if (prop.Name == "Category")
                {
                    dataGridView1.Columns.Add(new DataGridViewComboBoxColumn
                    {
                        Name = prop.Name,
                        HeaderText = prop.Name,
                        DataPropertyName = prop.Name,
                        DataSource = DataModel.Details.Keys.ToList()
                    });
                }
                else if (prop.Name == "Detail")
                {
                    dataGridView1.Columns.Add(new DataGridViewComboBoxColumn
                    {
                        Name = prop.Name,
                        HeaderText = prop.Name,
                        DataPropertyName = prop.Name,
                        DataSource = DataModel.Details.Values.ToList()
                    });
                }
                else if (prop.Name == "Member")
                {
                    dataGridView1.Columns.Add(new DataGridViewComboBoxColumn
                    {
                        Name = prop.Name,
                        HeaderText = prop.Name,
                        DataPropertyName = prop.Name,
                        DataSource = DataModel.Members
                    });
                }
                else if (prop.Name == "Payway")
                {
                    dataGridView1.Columns.Add(new DataGridViewComboBoxColumn
                    {
                        Name = prop.Name,
                        HeaderText = prop.Name,
                        DataPropertyName = prop.Name,
                        DataSource = DataModel.Payways
                    });
                }
                else
                {
                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = prop.Name,
                        HeaderText = prop.Name,
                        DataPropertyName = prop.Name
                    });
                }
            }

            // 刪除按鈕
            string imgPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Image",
                "delete.png"
            );
            var deleteColumn = new DataGridViewImageColumn
            {
                Name = "刪除",
                Tag = "刪除",
                HeaderText = "刪除",
                Image = Image.FromFile(imgPath),
                ImageLayout = DataGridViewImageCellLayout.Zoom,
            };
            dataGridView1.Columns.Add(deleteColumn);

            for (int i = 0; i < 8; i++)
            {
                dataGridView1.Columns[i].Visible = false;
            }
            var rows = dataGridView1.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                DataModel.Details.TryGetValue(rows[i].Cells[11].Value.ToString(), out List<string> detail);
                rows[i].Cells[12] = new DataGridViewComboBoxCell()
                {
                    DataSource = detail
                };
                rows[i].Cells[10].Value = rows[i].Cells[2].Value;
                rows[i].Cells[11].Value = rows[i].Cells[3].Value;
                rows[i].Cells[12].Value = rows[i].Cells[4].Value;
                rows[i].Cells[14].Value = Image.FromFile(rows[i].Cells[6].Value?.ToString());
                rows[i].Cells[15].Value = Image.FromFile(rows[i].Cells[7].Value?.ToString());
            }

        }

        private void CommitData(object sender, EventArgs e)
        {
            var dgw = (DataGridView)sender;
            if (dgw.CurrentCell is DataGridViewComboBoxCell)
            {
                dgw.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
    }
}
