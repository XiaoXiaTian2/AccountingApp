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
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AccountingApp.Views
{
    public partial class 記一筆 : Form
    {
        public 記一筆()
        {
            InitializeComponent();
            category.DataSource = DataModel.Details.Keys.ToList();
            category.SelectedIndex = -1;
            category.SelectedIndexChanged += (s, e) =>
            {
                var selectedCategory = category.SelectedItem.ToString();
                detail.DataSource = DataModel.Details[selectedCategory];
            };
            member.DataSource = DataModel.Members;
            payway.DataSource = DataModel.Payways;

            pictureBox1.Image = Image.FromFile(@"image/upload.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            pictureBox2.Image = Image.FromFile(@"image/upload.png");
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

        }

        private void accounting_Click(object sender, EventArgs e)
        {

            //HW:
            //1. 研究 Dispose 方法具體如何使用? 以及 GC.Collect() 強迫記憶體回收
            //2. 在每一次圖片上船完成之後都要回收記憶體 確保記憶體都是能回到最初的樣子

            //迅雷下載:支援邊播放邊下載(轉乘Byte)
            //PPS:緩衝進度條 (Buffer)

            //P2P:點對點傳輸 => 沒有server , client <-> client
            //BT種子:將檔案切碎 平行處理下載來自不同 client 的資料

            //streaming 串流(m3u8): 將檔案再次切碎，再做下載
            if (!File.Exists("data.csv"))
            {
                FileStream stream = File.Create("data.csv");
                stream.Close();
            }

            //StreamReader reader = new StreamReader("data.csv");
            //bool writeHeader = true;
            //while (!reader.EndOfStream)
            //{
            //    string line = reader.ReadLine();
            //    if (line.Contains("日期"))
            //        writeHeader = false;
            //}
            //reader.Close();

            string folder = "E:\\csharp_course\\accounting";
            string imgFolder = $"{folder}\\img";
            if (!Directory.Exists(imgFolder))
                Directory.CreateDirectory(imgFolder);
            string path1 = Path.Combine(imgFolder, Guid.NewGuid().ToString() + ".jpg");
            string path2 = Path.Combine(imgFolder, Guid.NewGuid().ToString() + ".jpg");
            pictureBox1.Image.Save(path1);
            pictureBox2.Image.Save(path2);

            RecordModel data = new RecordModel()
            {
                Date = dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                Money = money.Text,
                Categories = category.Text,
                Details = detail.Text,
                Members = member.Text,
                Payways = payway.Text,
                Img1 = path1,
                Img2 = path2,
            };
            CSVHelper.Write<RecordModel>($"{folder}/data/data.csv", data);
            //StreamWriter streamWriter = new StreamWriter("data.csv", true);
            //if (writeHeader)
            //{
            //    streamWriter.WriteLine("日期,金額,類型,細項,對象,付款方式");
            //}

            //streamWriter.WriteLine(String.Join(",", strings));
            //streamWriter.Flush();
            //streamWriter.Close();

            StreamReader reader2 = new StreamReader("data.csv");
            while (!reader2.EndOfStream)
            {
                string line = reader2.ReadLine();
                Console.WriteLine(line);
            }
            reader2.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "圖片|*.png;*.jpg";
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                PictureBox pic = sender as PictureBox;
                pic.Image = Image.FromFile(openFileDialog.FileName);
            }
        }
    }
}
