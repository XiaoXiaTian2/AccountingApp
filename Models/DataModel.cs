using AccountingApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingApp.Models
{
    internal class DataModel
    {
        public static List<string> Categories = new List<string>()
        {
            Category.Food.ToString(),
            Category.Live.ToString(),
            Category.Move.ToString(),
            Category.Entirement.ToString(),
            Category.Life.ToString()
        };

        public static Dictionary<string, List<string>> Details = new Dictionary<string, List<string>>()
        {
            { Category.Food.ToString() , new List<string>() { "早餐", "午餐", "晚餐", "零食","飲料"} },
            { Category.Live.ToString() , new List<string>() { "房租", "水費", "電費", "瓦斯費"} },
            { Category.Move.ToString() , new List<string>() { "悠遊卡", "火車費", "計程車費"} },
            { Category.Entirement.ToString() , new List<string>() { "電影", "Spotify", "Netflix", "Disney+"} },
            { Category.Life.ToString() , new List<string>() { "日常用品", "服裝", "學習費用", "醫療", "信用卡費"} },
        };

        public static List<string> Members = new List<string>()
        {
            Member.自己.ToString(),
            Member.朋友.ToString(),
            Member.家人.ToString(),
            Member.同事.ToString(),
        };

        public static List<string> Payways = new List<string>()
        {
            Payway.現金.ToString(),
            Payway.信用卡.ToString(),
            Payway.點數.ToString(),
        };
    }
}
