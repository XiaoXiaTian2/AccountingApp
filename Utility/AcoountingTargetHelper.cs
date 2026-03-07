using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountingApp.Utility
{
    internal static class AcoountingTargetHelper
    {
        public static string GetAccountingFolder(DateTime date)
        {
            string folder = "E:\\csharp_course\\accounting";
            string folderScope = date.ToString("yyyyMMdd");
            return $"{folder}\\{folderScope}";
        }
    }
}
