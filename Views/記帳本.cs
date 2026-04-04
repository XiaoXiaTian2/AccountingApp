using AccountingApp.Attributes;
using AccountingApp.Enums;
using AccountingApp.Extensions;
using AccountingApp.Models;
using AccountingApp.Models.DAOs;
using AccountingApp.Models.DTOs;
using AccountingApp.Presenters;
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
using static AccountingApp.Contracts.AddRecordContract;
using static AccountingApp.Contracts.RecordBookContract;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace AccountingApp.Views
{
    public partial class 記帳本 : Form, IRecordBookView
    {
        private List<RecordModel> records;

        IRecordBookPresenter presenter;

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
                presenter = new ReordBookPresenter(this);
                presenter.GetRecords(start_time.Value, end_time.Value);

                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = records;

                this.ReRender();
            }, 2, 400);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var record = records[e.RowIndex];
            presenter.EditRecords(new AccountBookDTO()
            {
                Date = record.Date,
                Money = record.Money,
                Categories = record.Categories,
                Details = record.Details,
                Members = record.Members,
                Payways = record.Payways,
                Img1 = record.Img1,
                Img2 = record.Img2,
            });
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
                    var record = records[e.RowIndex];

                    dataGridView1.Rows[e.RowIndex].Cells
                       .OfType<DataGridViewImageCell>().ToList()
                       .ForEach(cel => ((Bitmap)(cel.Value)).Dispose());

                    presenter.DeleteAccount(new AccountBookDTO()
                    {
                        Date = record.Date,
                        Money = record.Money,
                        Categories = record.Categories,
                        Details = record.Details,
                        Members = record.Members,
                        Payways = record.Payways,
                        Img1 = record.Img1,
                        Img2 = record.Img2,
                    });

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

        public void OnGetRecords(List<AccountBookDTO> accountBookDTOs)
        {
            records = new List<RecordModel>();
            foreach (var record in accountBookDTOs)
            {
                records.Add(new RecordModel()
                {
                    Date = record.Date,
                    Money = record.Money.ToString(),
                    Members = record.Members,
                    Payways = record.Payways,
                    Categories = record.Categories,
                    Details = record.Details,
                    Img1 = record.Img1,
                    Img2 = record.Img2,
                });
            }
        }

        public void OnEditRecords(AccountBookDTO accountBookDTO)
        {
            //
        }

        public void OnDeleteAccount(List<AccountBookDTO> dtos)
        {
            List<RecordModel> recs = new List<RecordModel>();
            foreach (AccountBookDTO dto in dtos)
            {
                recs.Add(new RecordModel()
                {
                    Date = dto.Date,
                    Money = dto.Money,
                    Members = dto.Members,
                    Payways = dto.Payways,
                    Categories = dto.Categories,
                    Details = dto.Details,
                });
            }
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = records = recs;
        }

        /*------*/

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
