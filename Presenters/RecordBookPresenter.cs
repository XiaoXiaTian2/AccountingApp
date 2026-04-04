using AccountingApp.Models.DAOs;
using AccountingApp.Models.DTOs;
using AccountingApp.Models.Repositories;
using AccountingApp.Utility;
using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AccountingApp.Contracts.AddRecordContract;
using static AccountingApp.Contracts.RecordBookContract;

namespace AccountingApp.Presenters
{
    public class ReordBookPresenter : IRecordBookPresenter
    {
        IRecordBookView _view = null;
        IRecordRepository recordRepo = new RecordRepository();

        public ReordBookPresenter(IRecordBookView view)
        {
            _view = view;
        }

        public void GetRecords(DateTime start, DateTime end)
        {
            List<RecordModelDAO> daos = recordRepo.GetRecords(start, end);
            List<AccountBookDTO> accountBookDTOs = new List<AccountBookDTO>();

            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<RecordModelDAO, AccountBookDTO>()
                // 名稱相同可不寫
                //.ForMember(x => x.Date, y => y.MapFrom(o => o.Date))
                //.ForMember(x => x.Money, y => y.MapFrom(o => o.Money))
                //.ForMember(x => x.Members, y => y.MapFrom(o => o.Members))
                //.ForMember(x => x.Categories, y => y.MapFrom(o => o.Categories))
                //.ForMember(x => x.Details, y => y.MapFrom(o => o.Details))
                //.ForMember(x => x.Payways, y => y.MapFrom(o => o.Payways))
                );
            var mapper = config.CreateMapper();

            this._view.OnGetRecords(mapper.Map<List<AccountBookDTO>>(daos));
        }

        public void EditRecords(AccountBookDTO accountBookDTO)
        {
            RecordModelDAO newDAO = recordRepo.EditRecords(new RecordModelDAO()
            {
                Date = accountBookDTO.Date,
                Money = accountBookDTO.Money,
                Members = accountBookDTO.Members,
                Payways = accountBookDTO.Payways,
                Categories = accountBookDTO.Categories,
                Details = accountBookDTO.Details,
                Img1 = accountBookDTO.Img1,
                Img2 = accountBookDTO.Img2,
            });
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RecordModelDAO, AccountBookDTO>());
            var mapper = config.CreateMapper();

            this._view.OnEditRecords(mapper.Map<AccountBookDTO>(newDAO));
        }

        public void DeleteAccount(AccountBookDTO accountBookDTO)
        {
            List<RecordModelDAO> lists = recordRepo.DeleteAccount(new RecordModelDAO()
            {
                Date = accountBookDTO.Date,
                Money = accountBookDTO.Money,
                Members = accountBookDTO.Members,
                Payways = accountBookDTO.Payways,
                Categories = accountBookDTO.Categories,
                Details = accountBookDTO.Details,
                Img1 = accountBookDTO.Img1,
                Img2 = accountBookDTO.Img2,
            });
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RecordModelDAO, AccountBookDTO>());
            var mapper = config.CreateMapper();

            this._view.OnDeleteAccount(mapper.Map<List<AccountBookDTO>>(lists));
        }
    }
}
