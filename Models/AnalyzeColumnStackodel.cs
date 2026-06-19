using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingApp.Models
{
    public class AnalyzeColumnStackodel
    {
        public string key { get; set; }
        public DateTime date { get; set; }
        public int sum { get; set; }

        public AnalyzeColumnStackodel(DateTime date, string key, int sum)
        {
            this.key = key;
            this.date = date;
            this.sum = sum;
        }
    }
}
