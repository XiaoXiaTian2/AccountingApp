using AccountingApp.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountingApp.Views
{
    internal class SingletonForm
    {
        private static Form currentForm;
        public static Dictionary<string, Form> forms = new Dictionary<string, Form>();
        public SingletonForm() { }

        public static Form GetInstance(string formName)
        {
            if (currentForm != null) { currentForm.Hide(); }

            if (!forms.TryGetValue(formName, out currentForm))
            {
                Type formType = Type.GetType($"AccountingApp.Views.{formName}");
                currentForm = (Form)Activator.CreateInstance(formType);
                currentForm.FormClosing += ClosingForm;

                Navbar nbr = (Navbar)currentForm.GetType().GetField("navbar1", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(currentForm);
                nbr.DisableBtn(formName);

                forms.Add(formName, currentForm);
            }

            return currentForm;
        }

        private static void ClosingForm(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
