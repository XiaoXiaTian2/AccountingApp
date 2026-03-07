using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountingApp.Views
{
    public partial class ImageForm : Form
    {
        public ImageForm(string imgPath)
        {
            InitializeComponent();
            this.pictureBox1.Image = new Bitmap(new MemoryStream(File.ReadAllBytes(imgPath)));
            //Image.FromFile(imgPath);
            //new Bitmap(new FileStream(imgPath, FileMode.Open));
            //this.pictureBox1.Image = Image.FromFile(imgPath);
            this.FormClosing += ImageForm_FormClosing;
        }

        private void ImageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.pictureBox1.Image.Dispose();
        }
    }
}
