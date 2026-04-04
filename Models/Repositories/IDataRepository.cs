using AccountingApp.Models.DAOs;
using AccountingApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingApp.Models.Repositories
{
    internal interface IDataRepository
    {
        List<string> GetCategories();
        List<string> GetMembers();
        List<string> GetPayways();
        Dictionary<string, List<string>> GetDetails();
    }
}
