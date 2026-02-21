using AccountingApp.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingApp.Models
{
    internal class RecordModel
    {
        [DisplayName("日期")]
        public string Date { get; set; }
        [DisplayName("錢錢")]
        public string Money { get; set; }
        [DisplayName("成員")]
        [ComboBoxColumn]
        public string Members { get; set; }
        [DisplayName("種類")]
        [ComboBoxColumn]
        public string Categories { get; set; }
        [DisplayName("詳細")]
        [ComboBoxColumn]
        public string Details { get; set; }
        [DisplayName("付款方式")]
        [ComboBoxColumn]
        public string Payways { get; set; }
        [DisplayName("圖片一")]
        [ImageColumn]
        public string Img1 { get; set; }
        [DisplayName("圖片二")]
        [ImageColumn]
        public string Img2 { get; set; }
    }
}
