using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.ViewModels;
using NPOI.SS.UserModel;

namespace Coevery.PropertyManagement.Services
{
    public interface IReportServices : IDependency
    {
        IEnumerable<ReceiptListViewModel> GenerateReceiptListViewModels(IList<BillRecord> billRecords);
        IEnumerable<BillReportSummaryViewModel> GenerateReceiptReportSummaryViewModel(IList<PaymentPartRecord> paymentRecords);

        IEnumerable<BillReportListViewModel> GenerateBillReportListViewModels(IList<BillRecord> billRecords);
        IEnumerable<BillReportSummaryViewModel> GenerateBillReportSummaryViewModel(IList<BillRecord> billRecords);

        IEnumerable<HouseStatusReportViewModel> GenerateHouseStatusReportViewModel(IList<HousePartRecord> houseRecords);
        IEnumerable<HouseSummaryReportViewModel> GenerateHouseSummaryReportViewModel(IList<HousePartRecord> houseRecords);

        IEnumerable<StatementReportListViewModel> GenerateStatementReportListViewModel(IList<BillRecord> billRecords);

        IEnumerable<BillAllocationReportListViewModel> GenerateBillAllocationReportListViewModel(IList<BillRecord> billRecords);
        void FillExcel<T>(string gridColumns, IEnumerable<T> data, ISheet sheet);
    }
}