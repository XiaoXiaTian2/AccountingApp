using AccountingApp.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountingApp.Components
{
    public partial class Navbar : UserControl
    {
        public Navbar()
        {
            InitializeComponent();

            IEnumerable<TypeInfo> types = Assembly.GetExecutingAssembly().DefinedTypes;
            //HW 過濾只含navbar的form & 當前btn disable
            List<TypeInfo> typeInfos = types.Where(type =>
            {
                return type.IsClass && type.IsSubclassOf(typeof(Form))
                && type.GetField("navbar1", BindingFlags.NonPublic | BindingFlags.Instance) != null;
            }).ToList();

            foreach (var type in typeInfos)
            {
                Button btn = new Button()
                {
                    Text = type.Name,
                    Margin = new Padding(0),
                    Padding = new Padding(0),
                };
                btn.Click += ChangePage_Click;
                flowLayoutPanel1.Controls.Add(btn);
            }
        }

        private void ChangePage_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            string page = btn.Text;
            SingletonForm.GetInstance(page).Show();
        }

        public void DisableBtn(string formName)
        {
            Button disableBtn = flowLayoutPanel1.Controls.OfType<Button>().FirstOrDefault(btn => btn.Text == formName);
            disableBtn.Enabled = false;
        }

        private void Navbar_Resize(object sender, EventArgs e)
        {
            int count = flowLayoutPanel1.Controls.OfType<Button>().Count();
            flowLayoutPanel1.Controls.OfType<Button>().ToList().ForEach(btn =>
            {
                btn.Width = this.Width / count;
                btn.Height = this.Height;
            });
        }
    }
}
