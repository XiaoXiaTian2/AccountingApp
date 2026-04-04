using AccountingApp.Enums;
using AccountingApp.Models;
using AccountingApp.Models.DTOs;
using AccountingApp.Presenters;
using AccountingApp.Utility;
using CSV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AccountingApp.Contracts.AddRecordContract;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace AccountingApp.Views
{
    public partial class 記一筆 : Form, IAddRecordView
    {
        IAddRecordPresenter presenter;

        public 記一筆()
        {
            InitializeComponent();

            presenter = new AddAccountPresenter(this);
            presenter.GetComboxDataDTO();

            pictureBox1.Image = Image.FromFile(@"image/upload.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            pictureBox2.Image = Image.FromFile(@"image/upload.png");
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        public void OnGetComboxDataDTO(ComboxDataDTO comboxDataDTO)
        {
            category.DataSource = comboxDataDTO.Details.Keys.ToList();
            category.SelectedIndex = -1;
            category.SelectedIndexChanged += (s, e) =>
            {
                var selectedCategory = category.SelectedItem.ToString();
                detail.DataSource = comboxDataDTO.Details[selectedCategory];
            };
            member.DataSource = comboxDataDTO.Members;
            payway.DataSource = comboxDataDTO.Payways;
        }

        public bool OnRecordCreated(bool success)
        {
            // 初始化
            money.Text = "";
            category.Text = DataModel.Categories[0];
            DataModel.Details.TryGetValue(category.Text, out List<string> details);
            detail.Text = details[0];
            member.Text = DataModel.Members[0];
            payway.Text = DataModel.Payways[0];
            pictureBox1.Image.Dispose();
            pictureBox1.Image = Image.FromFile(@"image/upload.png");
            pictureBox2.Image.Dispose();
            pictureBox2.Image = Image.FromFile(@"image/upload.png");
            GC.Collect();

            MessageBox.Show(success.ToString());
            return success;
        }

        private void accounting_Click(object sender, EventArgs e)
        {
            //HW:
            //1. 研究 Dispose 方法具體如何使用? 以及 GC.Collect() 強迫記憶體回收
            //2. 在每一次圖片上船完成之後都要回收記憶體 確保記憶體都是能回到最初的樣子

            //迅雷下載:支援邊播放邊下載(轉成Byte)
            //PPS:緩衝進度條 (Buffer)

            //P2P:點對點傳輸 => 沒有server , client <-> client
            //BT種子:將檔案切碎 平行處理下載來自不同 client 的資料

            //streaming 串流(m3u8): 將檔案再次切碎，再做下載

            // DTO
            RecordModelDTO data = new RecordModelDTO()
            {
                Date = dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                Money = money.Text,
                Categories = category.Text,
                Details = detail.Text,
                Members = member.Text,
                Payways = payway.Text,
                Img1 = pictureBox1.Image,
                Img2 = pictureBox2.Image,
            };
            presenter.CreateRecord(data);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "圖片|*.png;*.jpg"
            };
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                PictureBox pic = sender as PictureBox;
                pic.Image.Dispose();
                pic.Image = null;
                GC.Collect();
                pic.Image = Image.FromFile(openFileDialog.FileName);
            }
        }
    }
}
