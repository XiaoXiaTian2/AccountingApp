using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingApp.Models
{
    public class AnalyzeLineChartModel
    {
        //public string key { get; set; }
        public DateTime date { get; set; }
        public int sum { get; set; }

        public AnalyzeLineChartModel(DateTime date, int sum)
        {
            //this.key = key;
            this.date = date;
            this.sum = sum;
        }
    }
}
