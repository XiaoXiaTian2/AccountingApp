using AccountingApp.Models.DAOs;
using AccountingApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingApp.Contracts
{
    public class RecordBookContract
    {
        public interface IRecordBookPresenter
        {
            void GetRecords(DateTime start, DateTime end);
            void EditRecords(AccountBookDTO accountBookDTO);
            void DeleteAccount(AccountBookDTO accountBookDTO);
        }

        public interface IRecordBookView
        {
            void OnGetRecords(List<AccountBookDTO> recordModelDTOs);
            void OnEditRecords(AccountBookDTO accountBookDTO);
            void OnDeleteAccount(List<AccountBookDTO> accountBookDTOS);
        }
    }
}
