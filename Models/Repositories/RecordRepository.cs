using AccountingApp.Models.DAOs;
using AccountingApp.Models.DTOs;
using AccountingApp.Utility;
using CSV;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingApp.Models.Repositories
{
    internal class RecordRepository : IRecordRepository
    {
        private static string baseFolder = AcoountingTargetHelper.BaseFolder;

        public bool CreateAccount(RecordModelDAO accountDAO)
        {
            string basePath = Path.Combine(baseFolder, accountDAO.Date);
            CSVHelper.Write<RecordModelDAO>($"{basePath}/data/data.csv", accountDAO);

            return true;
        }

        public List<RecordModelDAO> GetRecords(DateTime start, DateTime end)
        {
            var diff = end.Date - start.Date;
            List<RecordModelDAO> records = new List<RecordModelDAO>();
            for (int i = 0; i <= diff.Days; i++)
            {
                string basePath = Path.Combine(baseFolder, start.AddDays(i).ToString("yyyy-MM-dd"));
                //string basePath = AcoountingTargetHelper.GetAccountingFolder(start_time.Value.AddDays(i));
                //var day = start_time.Value.AddDays(i).ToString("yyyyMMdd");
                string filePath = $"{basePath}/data/data.csv";
                if (File.Exists(filePath))
                {
                    records.AddRange(CSVHelper.Read<RecordModelDAO>(filePath));
                }
            }
            return records;
        }

        public RecordModelDAO EditRecords(RecordModelDAO recordDAO)
        {
            List<RecordModelDAO> records = this.GetRecords(DateTime.Parse(recordDAO.Date), DateTime.Parse(recordDAO.Date));

            string filePath = Path.Combine(baseFolder, recordDAO.Date, "data/data.csv");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                FileStream stream = File.Create(filePath);
                stream.Close();
            }

            RecordModelDAO target = records.FirstOrDefault(rec => rec.Img1 == recordDAO.Img1);
            var props = target.GetType().GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                props[i].SetValue(target, props[i].GetValue(recordDAO));
            }

            CSVHelper.WriteList<RecordModelDAO>(filePath, records);

            return target;
        }

        public List<RecordModelDAO> DeleteAccount(RecordModelDAO recordDAO)
        {
            List<RecordModelDAO> records = this.GetRecords(DateTime.Parse(recordDAO.Date), DateTime.Parse(recordDAO.Date));

            string filePath = Path.Combine(baseFolder, recordDAO.Date, "data/data.csv");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                FileStream stream = File.Create(filePath);
                stream.Close();
            }

            List<RecordModelDAO> newRecords = records.Where(rec => rec.Img1 != recordDAO.Img1).ToList();
            File.Delete(recordDAO.Img1);
            File.Delete(recordDAO.Img2);
            File.Delete(recordDAO.Img1.Replace("small_", ""));
            File.Delete(recordDAO.Img2.Replace("small_", ""));
            CSVHelper.WriteList<RecordModelDAO>(filePath, newRecords);

            return newRecords;
        }
    }
}
