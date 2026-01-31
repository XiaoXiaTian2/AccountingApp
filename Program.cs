using AccountingApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountingApp
{
    [Flags]
    enum FileAccessType
    {
        None = 0, // 000
        Read = 1, // 001
        Write = 2, // 010
        Execute = 4 // 100


        //001
        //100
        //-----
        //101
        //100
        //---
        //100
    }


    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(SingletonForm.GetInstance("記一筆"));
        }
    }
}
