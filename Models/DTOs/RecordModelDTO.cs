using AccountingApp.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingApp.Models.DTOs
{
    public class RecordModelDTO
    {
        public string Date { get; set; }
        public string Money { get; set; }
        public string Members { get; set; }
        public string Categories { get; set; }
        public string Details { get; set; }
        public string Payways { get; set; }
        public Image Img1 { get; set; }
        public Image Img2 { get; set; }
    }
}
