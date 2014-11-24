using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.ViewModels;

namespace Coevery.PropertyManagement.Services
{
    public interface IBillServices : IDependency
    {
        IEnumerable<BillListViewModel> GenerateBillListViewModels(IList<ContractHouseRecord> contractHouseRecords);
    }
}
