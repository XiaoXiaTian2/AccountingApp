using AccountingApp.Models;
using AccountingApp.Models.DAOs;
using AccountingApp.Models.DTOs;
using AccountingApp.Models.Repositories;
using AccountingApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AccountingApp.Services
{
    internal class AnalyzeService
    {
        IRecordRepository accoutRepo = new RecordRepository();

        //61-615
        public List<AnalyzeDTO> AnalyzeData(DateTime start, DateTime end, Dictionary<string, List<string>> filters, List<string> groups)
        {
            List<RecordModelDAO> data = accoutRepo.GetRecords(start, end);

            var categoriesKeys = DataModel.Details.Keys.ToList(); // 類別層級
            // category 層級 filter 先合併成一組 OR
            var categoryFilters = filters.Where(x => categoriesKeys.Contains(x.Key)).ToList();
            if (categoryFilters.Count > 0)
            {
                data = data.Where(x => categoryFilters.Any(filter =>
                    filter.Key == x.Categories && filter.Value.Contains(x.Details)))
                    .ToList();
            }

            // 其他層級用 AND
            foreach (var filter in filters.Where(x => !categoriesKeys.Contains(x.Key)))
            {
                PropertyInfo prop = typeof(RecordModelDAO).GetProperty(filter.Key);
                if (prop == null)
                    continue;
                data = data.Where(x => filter.Value.Contains(prop.GetValue(x)?.ToString())).ToList();
            }

            var existingDetail = groups.Where(x => DataModel.Details.TryGetValue(x, out List<string> a))
                .ToDictionary(x => x, x =>
                {
                    DataModel.Details.TryGetValue(x, out List<string> b);
                    return b;
                });

            // category / member / payment 分群
            var groupData = groups.Count > 0
                ? data.Where(x =>
                {
                    if (existingDetail.Count > 0 && !existingDetail.ContainsKey(x.Categories))
                        return false;
                    return true;
                })
                .GroupBy(x =>
                {
                    var keyParts = new List<string>();
                    bool categoryKeyAdded = false;
                    foreach (var group in groups)
                    {
                        // 多個類別屬於同一個「類型」軸，只需算一次 x.Details，避免重複 key
                        if (categoriesKeys.Contains(group))
                        {
                            if (categoryKeyAdded)
                                continue;
                            keyParts.Add(x.Details);
                            categoryKeyAdded = true;
                            continue;
                        }

                        keyParts.Add(x.GetType().GetProperty(group).GetValue(x, null).ToString());
                    }
                    return string.Join(",", keyParts);
                })
                .Select(x => new AnalyzeDTO(x.Key, x.Sum(y => int.Parse(y.Money))))
                : data.Select(x => new AnalyzeDTO("全部", int.Parse(x.Money)))
                    .GroupBy(x => x.key).Select(x => new AnalyzeDTO(x.Key, x.Sum(y => y.sum)));
            return groupData.ToList();
        }

        public List<AnalyzeLineChartModel> AnalyzeChartData(DateTime start, DateTime end, Dictionary<string, List<string>> filters, List<string> groups)
        {
            List<RecordModelDAO> data = accoutRepo.GetRecords(start, end);

            var categoriesKeys = DataModel.Details.Keys.ToList(); // 類別層級
            // category 層級 filter 先合併成一組 OR
            var categoryFilters = filters.Where(x => categoriesKeys.Contains(x.Key)).ToList();
            if (categoryFilters.Count > 0)
            {
                data = data.Where(x => categoryFilters.Any(filter =>
                    filter.Key == x.Categories && filter.Value.Contains(x.Details)))
                    .ToList();
            }

            // 其他層級用 AND
            foreach (var filter in filters.Where(x => !categoriesKeys.Contains(x.Key)))
            {
                PropertyInfo prop = typeof(RecordModelDAO).GetProperty(filter.Key);
                if (prop == null)
                    continue;
                data = data.Where(x => filter.Value.Contains(prop.GetValue(x)?.ToString())).ToList();
            }

            var existingDetail = groups.Where(x => DataModel.Details.TryGetValue(x, out List<string> a))
                .ToDictionary(x => x, x =>
                {
                    DataModel.Details.TryGetValue(x, out List<string> b);
                    return b;
                });

            var diff = end.Date - start.Date;
            var rstart = start.AddDays(-1);
            List<DateTime> times = Enumerable.Range(1, diff.Days + 1)
                .Select(x =>
                {
                    rstart = rstart.AddDays(1);
                    return rstart;
                }).ToList();

            // date 分群
            var groupData = data.Where(x =>
            {
                if (existingDetail.Count > 0 && !existingDetail.ContainsKey(x.Categories))
                    return false;
                return true;
            })
                .GroupBy(x => x.Date)
                .Select(x => new AnalyzeLineChartModel(
                    DateTime.Parse(x.Key),
                    x.Sum(y => int.Parse(y.Money))
                )).ToList();

            for (int i = 0; i < times.Count; i++)
            {
                if (!groupData.Any(x => x.date == times[i].Date))
                    groupData.Add(new AnalyzeLineChartModel(
                        times[i],
                        0
                    ));
            }
            return groupData.OrderBy(x => x.date).ToList();
        }

        public Dictionary<string, Dictionary<string, string>> AnalyzeColumnStackData(DateTime start, DateTime end, Dictionary<string, List<string>> filters, List<string> groups)
        {
            List<RecordModelDAO> data = accoutRepo.GetRecords(start, end);

            var categoriesKeys = DataModel.Details.Keys.ToList(); // 類別層級
            // category 層級 filter 先合併成一組 OR
            var categoryFilters = filters.Where(x => categoriesKeys.Contains(x.Key)).ToList();
            if (categoryFilters.Count > 0)
            {
                data = data.Where(x => categoryFilters.Any(filter =>
                    filter.Key == x.Categories && filter.Value.Contains(x.Details)))
                    .ToList();
            }

            // 其他層級用 AND
            foreach (var filter in filters.Where(x => !categoriesKeys.Contains(x.Key)))
            {
                PropertyInfo prop = typeof(RecordModelDAO).GetProperty(filter.Key);
                if (prop == null)
                    continue;
                data = data.Where(x => filter.Value.Contains(prop.GetValue(x)?.ToString())).ToList();
            }

            var existingDetail = groups.Where(x => DataModel.Details.TryGetValue(x, out List<string> a))
                .ToDictionary(x => x, x =>
                {
                    DataModel.Details.TryGetValue(x, out List<string> b);
                    return b;
                });

            // category / member / payment 分群
            var datas = data.Where(x =>
            {
                if (existingDetail.Count > 0 && !existingDetail.ContainsKey(x.Categories))
                    return false;
                return true;
            })

                .GroupBy(x =>
                {
                    var keyParts = new List<string>();
                    bool categoryKeyAdded = false;
                    foreach (var group in groups)
                    {
                        // 多個類別屬於同一個「類型」軸，只需算一次 x.Details，避免重複 key
                        if (categoriesKeys.Contains(group))
                        {
                            if (categoryKeyAdded)
                                continue;
                            keyParts.Add(x.Details);
                            categoryKeyAdded = true;
                            continue;
                        }

                        keyParts.Add(x.GetType().GetProperty(group).GetValue(x, null).ToString());
                    }
                    return (x.Date, string.Join(",", keyParts));
                });

            var diff = end.Date - start.Date;
            var rstart = start.AddDays(-1);
            List<DateTime> times = Enumerable.Range(1, diff.Days + 1)
                .Select(x =>
                {
                    rstart = rstart.AddDays(1);
                    return rstart;
                }).ToList();

            var groupedList = datas.ToList();
            var keys = groupedList.Select(x => x.Key.Item2).Distinct().ToList();
            var allDates = times.Select(t => t.Date.ToString("MM-dd")).ToList();

            // 建立 <date, <group, money>> 的完整矩形，每個日期都含所有群組，預設 0
            Dictionary<string, Dictionary<string, string>> finalData = allDates
                .ToDictionary(date => date, date => keys.ToDictionary(key => key, key => "0"));

            // 填入各群組每日的加總
            foreach (var group in groupedList)
            {
                string date = DateTime.Parse(group.Key.Item1).ToString("MM-dd");
                if (finalData.ContainsKey(date))
                    finalData[date][group.Key.Item2] = group.Sum(x => int.Parse(x.Money)).ToString();
            }

            return finalData;
        }
    }
}
