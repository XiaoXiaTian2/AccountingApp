using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountingApp.Utility
{
    internal static class FormExtension
    {
        private static System.Threading.Timer timer = null;

        public static void DebounceClick(this Form form, Action callback, int due)
        {
            if (timer == null)
                timer = new System.Threading.Timer((state) => form.Invoke(callback), null, due, -1);
            timer.Change(due, -1);
        }

        public static void DebounceClick<T>(this Form form, Action<T> callback, T para, int due)
        {
            if (timer == null)
            {
                //_callback = callback;
                //form.Tag = state;
                timer = new System.Threading.Timer((state) => form.Invoke(callback, para), null, due, -1);
            }
            timer.Change(due, -1);
        }
    }
}
