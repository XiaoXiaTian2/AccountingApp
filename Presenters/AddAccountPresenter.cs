using AccountingApp.Models.DAOs;
using AccountingApp.Models.DTOs;
using AccountingApp.Models.Repositories;
using AccountingApp.Utility;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AccountingApp.Contracts.AddRecordContract;

namespace AccountingApp.Presenters
{
    internal class AddAccountPresenter : IAddRecordPresenter
    {
        private static string baseFolder = AcoountingTargetHelper.BaseFolder;
        IAddRecordView _view = null;
        IRecordRepository accoutRepo = new RecordRepository();
        IDataRepository dataRepo = new DataRepository();

        public AddAccountPresenter(IAddRecordView view)
        {
            _view = view;
        }

        void IAddRecordPresenter.CreateRecord(RecordModelDTO record)
        {
            if (!File.Exists("data.csv"))
            {
                FileStream stream = File.Create("data.csv");
                stream.Close();
            }
            string imgFolder = Path.Combine(baseFolder, record.Date, "img");
            if (!Directory.Exists(imgFolder))
                Directory.CreateDirectory(imgFolder);

            string img1 = Guid.NewGuid().ToString();
            string img2 = Guid.NewGuid().ToString();
            string path1 = Path.Combine(imgFolder, $"small_{img1}.jpg"); // 40*40
            string path3 = Path.Combine(imgFolder, $"{img1}.jpg");
            string path2 = Path.Combine(imgFolder, $"small_{img2}.jpg");
            string path4 = Path.Combine(imgFolder, $"{img2}.jpg");

            using (Image thumb1 = ImgCompressor.ResizeImage(record.Img1, 40, 40))
            {
                thumb1.Save(path1);
            }
            using (Image compress1 = ImgCompressor.GetCompressedImage(record.Img1))
            {
                compress1.Save(path3);
            }

            using (Image thumb2 = ImgCompressor.ResizeImage(record.Img2, 40, 40))
            {
                thumb2.Save(path2);
            }
            using (Image compress2 = ImgCompressor.GetCompressedImage(record.Img2))
            {
                compress2.Save(path4);
            }

            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<RecordModelDTO, RecordModelDAO>()
                .ForMember(x => x.Img1, y => y.MapFrom(o => path1))
                .ForMember(x => x.Img2, y => y.MapFrom(o => path2))
            );
            var mapper = config.CreateMapper();
            RecordModelDAO recordModelDAO = mapper.Map<RecordModelDAO>(record);

            bool success = accoutRepo.CreateAccount(recordModelDAO);

            this._view.OnRecordCreated(success);
        }

        public void GetComboxDataDTO()
        {
            ComboxDataDTO dto = new ComboxDataDTO()
            {
                Categories = dataRepo.GetCategories(),
                Details = dataRepo.GetDetails(),
                Members = dataRepo.GetMembers(),
                Payways = dataRepo.GetPayways(),
            };

            this._view.OnGetComboxDataDTO(dto);
        }
    }
}
