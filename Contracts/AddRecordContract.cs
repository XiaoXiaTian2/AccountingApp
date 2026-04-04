using AccountingApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingApp.Contracts
{
    internal class AddRecordContract
    {
        public interface IAddRecordPresenter
        {
            void CreateRecord(RecordModelDTO record);
            void GetComboxDataDTO();
        }

        public interface IAddRecordView
        {
            bool OnRecordCreated(bool success);
            void OnGetComboxDataDTO(ComboxDataDTO comboxDataDTO);
        }
    }
}
