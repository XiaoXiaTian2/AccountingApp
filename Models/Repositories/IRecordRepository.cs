using AccountingApp.Models.DAOs;
using AccountingApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingApp.Models.Repositories
{
    internal interface IRecordRepository
    {
        bool CreateAccount(RecordModelDAO accountDAO);
        List<RecordModelDAO> GetRecords(DateTime start, DateTime end);
        RecordModelDAO EditRecords(RecordModelDAO recordDAO);
        List<RecordModelDAO> DeleteAccount(RecordModelDAO accountDAO);
    }
}
