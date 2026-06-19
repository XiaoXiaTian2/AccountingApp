using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingApp.Models.DAOs
{
    public class RecordModelDAO
    {
        public string Date { get; set; }
        public string Money { get; set; }
        [DisplayName("Member")]
        public string Members { get; set; }
        [DisplayName("Category")]
        public string Categories { get; set; }
        public string Details { get; set; }
        [DisplayName("Payway")]
        public string Payways { get; set; }
        public string Img1 { get; set; }
        public string Img2 { get; set; }
    }
}
