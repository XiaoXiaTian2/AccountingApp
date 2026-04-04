using AccountingApp.Attributes;
using AccountingApp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingApp.Models.DTOs
{
    public class ComboxDataDTO
    {
        public List<string> Categories { get; set; }

        public Dictionary<string, List<string>> Details { get; set; }

        public List<string> Members { get; set; }

        public List<string> Payways { get; set; }
    }
}
