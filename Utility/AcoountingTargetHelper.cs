using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountingApp.Utility
{
    internal static class AcoountingTargetHelper
    {
        // 記帳資料根目錄，由 App.config 的 AccountingFolder 設定，相對路徑以執行檔所在資料夾為基準
        public static string BaseFolder { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["AccountingFolder"] ?? "accounting");

        public static string GetAccountingFolder(DateTime date)
        {
            string folder = BaseFolder;
            string folderScope = date.ToString("yyyyMMdd");
            return $"{folder}\\{folderScope}";
        }
    }
}
