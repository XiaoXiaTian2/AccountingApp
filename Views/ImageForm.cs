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
            //Image.FromFile(imgPath);
            this.pictureBox1.Image = new Bitmap(new MemoryStream(File.ReadAllBytes(imgPath)));
            //new Bitmap(new FileStream(imgPath, FileMode.Open));
            //this.pictureBox1.Image = Image.FromFile(imgPath);
        }
    }
}
