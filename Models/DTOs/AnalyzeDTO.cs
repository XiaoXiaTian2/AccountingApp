using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingApp.Models.DTOs
{
    public class AnalyzeDTO
    {
        public string key { get; set; }
        public int sum { get; set; }

        public AnalyzeDTO(string key, int sum)
        {
            this.key = key;
            this.sum = sum;
        }
    }
}
