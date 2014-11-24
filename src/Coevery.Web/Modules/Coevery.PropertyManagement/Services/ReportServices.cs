using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Coevery.Data;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.ViewModels;
using Newtonsoft.Json.Schema;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using Coevery.PropertyManagement.Extensions;

namespace Coevery.PropertyManagement.Services
{
    public class ReportServices : IReportServices
    {
        private readonly IRepository<ChargeItemSettingCommonRecord> _chargeItemSettingCommonRepository;
        private readonly IRepository<ContractHouseRecord> _contractHouseRecordRepository;
        private readonly IRepository<MeterTypePartRecord> _meterTypeRepository;
        private readonly IRepository<CustomerPartRecord> _customerRepository;
        private readonly IRepository<PaymentLineItemPartRecord> _paymentLineItemRepository;
        private readonly IRepository<PaymentMethodItemRecord> _paymentMethodItemRepository;
        public ReportServices(
            IRepository<ContractHouseRecord> contractHouseRecordRepository,
            IRepository<MeterTypePartRecord> meterTypeRepository,
            IRepository<CustomerPartRecord> customerRepository, 
            IRepository<PaymentLineItemPartRecord> paymentLineItemRepository,
            IRepository<ChargeItemSettingCommonRecord> chargeItemSettingCommonRepository,
            IRepository<PaymentMethodItemRecord> paymentMethodItemRepository)
        {
            _contractHouseRecordRepository = contractHouseRecordRepository;
            _meterTypeRepository = meterTypeRepository;
            _customerRepository = customerRepository;
            _paymentLineItemRepository = paymentLineItemRepository;
            _chargeItemSettingCommonRepository = chargeItemSettingCommonRepository;
            _paymentMethodItemRepository = paymentMethodItemRepository;
        }

        /// <summary>
        /// 生成收费明细表
        /// </summary>
        /// <param name="billRecords"></param>
        /// <returns></returns>
        public IEnumerable<ReceiptListViewModel> GenerateReceiptListViewModels(IList<BillRecord> billRecords) {

            var list = (from bill in billRecords
                        select new ReceiptListViewModel {
                            ChargeItemAmount = bill.Status == BillRecord.BillStatusOption.已结账单 ? bill.Amount : null,
                            ChargeItemSettingName = bill.ChargeItem.Name,
                            ContractNumber = bill.Contract.Number,
                            HouseNumber = bill.House.HouseNumber,
                            HouseOwnerName = bill.House.Owner.Name,
                            HouseRenterName = bill.Contract.Renter.Name
                        }).ToList();
            
            return list;
        }
        /// <summary>
        /// 生成收费单明细汇总表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BillReportSummaryViewModel> GenerateReceiptReportSummaryViewModel(IList<PaymentPartRecord> paymentRecords) {

            var paymentList = (from p in paymentRecords.SelectMany(p => p.PaymentMethodItems)
                               group p by p.PaymentMethod
                               into result
                               select new BillReportSummaryViewModel() {
                                   ChargeItemName = result.Key.ToString(),
                                   ReceivedMoney = result.Sum(x => x.Amount)
                               }).ToList();
            if(paymentList.Count>0) {
                paymentList.Add(new BillReportSummaryViewModel() {
                    ChargeItemName = "合计",
                    ReceivedMoney = paymentList.Sum(p => p.ReceivedMoney)
                });
            }

            var chargeItemList = GenerateBillReportSummaryViewModel(paymentRecords.SelectMany(p => p.LineItems).Select(p => p.Bill).ToList());

            return paymentList.Concat(chargeItemList);



            //todo: Summary the New Table Payment Type
            return new List<BillReportSummaryViewModel>() {
                new BillReportSummaryViewModel() {
                    ChargeItemName = "现金", ReceivedMoney = 10
                },
                new BillReportSummaryViewModel() {
                    ChargeItemName = "划卡", ReceivedMoney = 20
                },
                new BillReportSummaryViewModel() {
                    ChargeItemName = "网银打款", ReceivedMoney = 30
                },
                new BillReportSummaryViewModel() {
                    ChargeItemName = "合计", ReceivedMoney = 60
                },
                new BillReportSummaryViewModel() {
                    ChargeItemName = "租金", ReceivedMoney = 100
                },
                new BillReportSummaryViewModel() {
                    ChargeItemName = "物业管理费", ReceivedMoney = 60
                },
                new BillReportSummaryViewModel() {
                    ChargeItemName = "水电费", ReceivedMoney = 100
                },
                new BillReportSummaryViewModel() {
                    ChargeItemName = "合计", ReceivedMoney = 260
                }
            };
        }

        /// <summary>
        /// 生成账单明细表，或欠费明细表
        /// </summary>
        /// <param name="billRecords"></param>
        /// <returns></returns>
        public IEnumerable<BillReportListViewModel> GenerateBillReportListViewModels(IList<BillRecord> billRecords) {

            var list = (from bill in billRecords
                        select new BillReportListViewModel() {
                            ContractNumber = bill.Contract.Number,
                            HouseOwnerName = bill.House.Owner.Name,
                            HouseRenterName = bill.Contract.Renter.Name,
                            OfficerName = bill.House.Officer.UserName,
                            HouseNumber = bill.House.HouseNumber,
                            ExpenserName = GetExpenserName(bill),
                            ChargeItemSettingName = bill.ChargeItem.Name,
                            ChargeTerm = bill.Month < 10 ? string.Format("{0}年0{1}月", bill.Year, bill.Month) : string.Format("{0}年{1}月", bill.Year, bill.Month),
                            ChargeUnit = GetChargeUnit(bill.ChargeItem),
                            ChargeNumber = bill.Quantity,
                            ChargeUnitPrice = bill.ChargeItem.UnitPrice,
                            ChargeItemAmount = bill.Amount,
                            BillStatus = bill.Status == BillRecord.BillStatusOption.已结账单 ? "是" : "否"
                        });

            return list;
        }
        /// <summary>
        /// 生成账单明细汇总表，或欠费明细汇总表
        /// </summary>
        /// <param name="billRecords"></param>
        /// <returns></returns>
        public IEnumerable<BillReportSummaryViewModel> GenerateBillReportSummaryViewModel(IList<BillRecord> billRecords) {

            var query = from item in
                           (from bill in billRecords
                            select new {
                                ChargeItemName = bill.ChargeItem.Name, 
                                bill.Amount, 
                                bill.Status,
                            })
                       group item by item.ChargeItemName
                       into result
                       select new BillReportSummaryViewModel() {
                           ChargeItemName = result.Key,
                           Amount = result.Sum(l => l.Amount),
                           ReceivedMoney = result.Where(l => l.Status == BillRecord.BillStatusOption.已结账单).Sum(l => l.Amount),
                           OwingMoney = result.Where(l => l.Status != BillRecord.BillStatusOption.已结账单).Sum(l => l.Amount)
                       };

            List<BillReportSummaryViewModel> list = query.ToList();
            if (list.Count > 0) {
                list.Add(new BillReportSummaryViewModel() {
                    ChargeItemName = "合计",
                    Amount = list.Sum(b => b.Amount),
                    ReceivedMoney = list.Sum(b => b.ReceivedMoney),
                    OwingMoney = list.Sum(b => b.OwingMoney)
                });
            }

            return list.ToList();
        }

        /// <summary>
        /// 生成房间经营状态表
        /// </summary>
        /// <param name="houseRecords"></param>
        /// <returns></returns>
        public IEnumerable<HouseStatusReportViewModel> GenerateHouseStatusReportViewModel(IList<HousePartRecord> houseRecords) {

            var list = from h in houseRecords
                       join c in _contractHouseRecordRepository.Table on h.Id equals c.House.Id into result
                       from r in result.DefaultIfEmpty()
                       select new HouseStatusReportViewModel() {
                           ApartmentName = h.Apartment.Name,
                           BuildingName = h.Building.Name,
                           HouseNumber = h.HouseNumber,
                           ContractNumber = r == null ? "" : r.Contract.Number,
                           HouseOwnerName = h.Owner.Name,
                           HouseRenterName = r == null ? "" : r.Contract.Renter.Name,
                           ExpenserName = "", //todo: one house will have more than one Expenser.
                           OfficerName = h.Officer.UserName,
                           BuildingArea = h.BuildingArea,
                           InsideArea = h.InsideArea,
                           PoolArea = h.PoolArea,
                           HouseStatus = h.HouseStatus.ToString()
                       };

            return list;
        }
        /// <summary>
        /// 生成房间经营状态汇总表
        /// </summary>
        /// <param name="houseRecords"></param>
        /// <returns></returns>
        public IEnumerable<HouseSummaryReportViewModel> GenerateHouseSummaryReportViewModel(IList<HousePartRecord> houseRecords) {
            var query = from s in
                           (from h in houseRecords
                            select new {
                                HouseStatus = h.HouseStatus.ToString(),
                                h.BuildingArea,
                                h.InsideArea,
                                h.PoolArea
                            })
                       group s by s.HouseStatus
                       into result
                       select new HouseSummaryReportViewModel() {
                           HouseStatus = result.Key,
                           BuildingArea = result.Sum(h => h.BuildingArea),
                           InsideArea = result.Sum(h => h.InsideArea),
                           PoolArea = result.Sum((h => h.PoolArea))
                       };

            List<HouseSummaryReportViewModel> list = query.ToList();
            list.Add(new HouseSummaryReportViewModel() {
                HouseStatus = "合计",
                BuildingArea = list.Sum(h => h.BuildingArea),
                InsideArea = list.Sum(h => h.InsideArea),
                PoolArea = list.Sum(h => h.PoolArea)
            });

            return list;
        }


        public IEnumerable<StatementReportListViewModel> GenerateStatementReportListViewModel(IList<BillRecord> billRecords) {
            // get bill
            var billList = from b in billRecords
                           select new StatementReportListViewModel() {
                               Date = b.StartDate,
                               ChargeItemName = b.ChargeItem.Name,
                               ChargeTerm = b.Month < 10 ? string.Format("{0}年0{1}月", b.Year, b.Month) : string.Format("{0}年{1}月", b.Year, b.Month),
                               ChargeItemAmount = b.Amount
                           };

            // get receipt
            var paymentQuery = (from b in billRecords
                                join p in _paymentLineItemRepository.Table on b.Id equals p.Bill.Id
                                where b.Status == BillRecord.BillStatusOption.已结账单
                                select p.Payment).Distinct().SelectMany(p => p.PaymentMethodItems);
            var receivedList = from p in paymentQuery
                               select new StatementReportListViewModel() {
                                   Date = p.Payment.PaidOn,
                                   PaymentMethod = p.PaymentMethod.ToString(),
                                   ReceivedAmount = p.Amount,
                               };

            var totalList = billList.Concat(receivedList).OrderBy(x=>x.Date).ToList();

            // calculate the owning money for each statement.
            decimal? clientOwingAmount = 0; // todo: 默认客户余额为0，如果不为0，需要计算这次查询之前的余额
            for (int i = 0; i < totalList.Count; i++) {
                totalList[i].OwingAmount = i == 0 ? clientOwingAmount : totalList[i - 1].OwingAmount;
                if (totalList[i].ChargeItemAmount.HasValue) totalList[i].OwingAmount = totalList[i].OwingAmount + totalList[i].ChargeItemAmount;
                if (totalList[i].ReceivedAmount.HasValue) totalList[i].OwingAmount = totalList[i].OwingAmount - totalList[i].ReceivedAmount;
            }

            return totalList;
        }

        public IEnumerable<BillAllocationReportListViewModel> GenerateBillAllocationReportListViewModel(
            IList<BillRecord> billRecords)
        {

            var query = from s in
                (from b in billRecords
                    join p in _paymentLineItemRepository.Table on b.Id equals p.Bill.Id
                    where b.Status == BillRecord.BillStatusOption.已结账单
                    select new
                    {
                        CustomerName = GetExpenserName(b),
                        ContractNumber = b.Contract.Number,
                        b.House.HouseNumber,
                        ChargeItemName = b.ChargeItem.Name,
                        b.Amount,
                        paymentamount = p.Payment.Paid,
                        b.Month
                    }).Distinct()
                group s by s.ChargeItemName;
               var queryGroups = query.ToList();
               var advancePaymentAmount = GetBillsAdvancePaymentAmount(billRecords); //预付款金额
               var list =new List<BillAllocationReportListViewModel>();
               foreach (var group in queryGroups)
               {
                   var currentPeriodAmount = GetPaidCurrentPeriod(billRecords, group.Key); //非预付款情况，取当月费用
                   var billAllocationReportListView =new BillAllocationReportListViewModel();
                   billAllocationReportListView.ChargeItemName = group.Key;
                   billAllocationReportListView.CustomerName = group.FirstOrDefault().CustomerName;
                   billAllocationReportListView.ContractNumber = group.FirstOrDefault().ContractNumber;
                   billAllocationReportListView.HouseNumber =
                       group.Select(x => x.HouseNumber).Distinct().Aggregate((i, j) => i + "," + j);

                   //单独用预付款冲完所有账单情况
                   if(IsFullAdvancePayment(billRecords))
                   {
                       //实收本期
                       billAllocationReportListView.PaidCurrentPeriod = 0;
                       //实收往期
                       billAllocationReportListView.PaidPreviousPeriod = 0;
                       var leftBalanceAmount = GetLeftBanace(billRecords, group.Key);       //GetLeftBanace方法支持用预付款单独冲完全部账单预付款情况，取当月费用
                       //往期结转
                       billAllocationReportListView.LeftBalance = leftBalanceAmount;
                   }
                   else if (IsMixedPayment(billRecords))
                   {   //包含预付款的混合付款方式 从开头到后面的分配，按费用项目顺序来分
                       var chargeItemAmount = group.Sum(x => x.Amount) - currentPeriodAmount;//收费项目费用(除开当月费用后)的总计
                       if (advancePaymentAmount >= chargeItemAmount)
                       {
                           //往期结转
                           billAllocationReportListView.LeftBalance = chargeItemAmount;
                           advancePaymentAmount -= chargeItemAmount;
                           //实收往期
                           billAllocationReportListView.PaidPreviousPeriod = 0;
                       }
                       else
                       {    //往期结转
                           billAllocationReportListView.LeftBalance = advancePaymentAmount;
                           //实收往期
                           billAllocationReportListView.PaidPreviousPeriod = currentPeriodAmount - advancePaymentAmount;//减去预缴的部分得到差额为实收往期
                           advancePaymentAmount = 0;
                       }
                       //实收本期
                       billAllocationReportListView.PaidCurrentPeriod = currentPeriodAmount;
                       
                   }
                   else
                   {   //非预付款方式(现金转账网银不包含预付款)
                       //实收本期
                       billAllocationReportListView.PaidCurrentPeriod = currentPeriodAmount;
                       //实收往期
                       billAllocationReportListView.PaidPreviousPeriod = group.Sum(x => x.Amount) - currentPeriodAmount;
                   }

                   list.Add(billAllocationReportListView);
               }
                        
            return list;
        }

        //取缴费当月应收款费用(分预付款和不是预付款)
        public decimal? GetPaidCurrentPeriod(IList<BillRecord> billRecords,string chargeItemName)
        {
        var billQuery = (from b in billRecords
                             join p in _paymentLineItemRepository.Table on b.Id equals p.Bill.Id
                             join m in _paymentMethodItemRepository.Table on p.Payment.Id equals m.Payment.Id
                             where b.Status == BillRecord.BillStatusOption.已结账单
                             && p.Payment.PaidOn > b.StartDate
                             && p.Payment.PaidOn < b.EndDate
                             && b.ChargeItem.ChargeItemSetting.Name == chargeItemName
                             && m.PaymentMethod!=PaymentMethodOption.预付款  //费预付款的情况，算出实收本月费用
                             select b);
            var paidCurrentPeriod = billQuery.ToList().GroupBy(x=>x.ChargeItem.Name)
                .Select(g=>new
                {
                    chargeItemName=g.Key,
                    amount=g.Sum(x=>x.ChargeItem.ChargingPeriod != null ? x.Amount/(int)x.ChargeItem.ChargingPeriod : null)
                });
          
            var firstOrDefault = paidCurrentPeriod.FirstOrDefault();
            if (firstOrDefault != null) return firstOrDefault.amount;
           
            return 0;
        }
        // 获取结转往期费用
        public decimal? GetLeftBanace(IList<BillRecord> billRecords, string chargeItemName)
        {
            //1.单独用预付款冲完所有账单
            var advanceBillQuery = (from b in billRecords
                             join p in _paymentLineItemRepository.Table on b.Id equals p.Bill.Id
                             join m in _paymentMethodItemRepository.Table on p.Payment.Id equals m.Payment.Id
                             where b.Status == BillRecord.BillStatusOption.已结账单
                             && b.ChargeItem.ChargeItemSetting.Name == chargeItemName
                             && m.PaymentMethod == PaymentMethodOption.预付款
                             && p.Payment.Paid == m.Amount  //如果Payment表中金额等于paymentmethod表中金额就是预付款冲完所有账单
                             select b);
            var billQuery = advanceBillQuery as IList<BillRecord> ?? advanceBillQuery.ToList();
            if (billQuery.ToList().Count > 0){
               var paidLeftBanace = billQuery.ToList().GroupBy(x => x.ChargeItem.Name)
                .Select(g => new
                {
                    chargeItemName=g.Key,
                    amount=g.Sum(x=>x.Amount)
                });
                return paidLeftBanace.FirstOrDefault().amount;
            }
      
            return 0;
        }
        ////判断是否为包含预付款的混合方式
        private bool IsMixedPayment(IList<BillRecord> billRecords)
        {
            var paymentMethodItemsQuery = (from b in billRecords
                                           join p in _paymentLineItemRepository.Table on b.Id equals p.Bill.Id
                                           where b.Status == BillRecord.BillStatusOption.已结账单
                                           select p.Payment).Distinct().SelectMany(p => p.PaymentMethodItems);
            var paymentMethodRecord = paymentMethodItemsQuery.ToList();
            if (paymentMethodRecord.Count > 1 &&
                paymentMethodRecord.Exists(x => x.PaymentMethod == PaymentMethodOption.预付款))
            {
                return true;
            }
            return false;
        }
        //1.判断是否单独用预付款冲完所有账单
        private bool IsFullAdvancePayment(IList<BillRecord> billRecords)
        {
             var advanceBillQuery = (from b in billRecords
                             join p in _paymentLineItemRepository.Table on b.Id equals p.Bill.Id
                             join m in _paymentMethodItemRepository.Table on p.Payment.Id equals m.Payment.Id
                             where b.Status == BillRecord.BillStatusOption.已结账单
                             && m.PaymentMethod == PaymentMethodOption.预付款
                             && p.Payment.Paid == m.Amount  //如果Payment表中金额等于paymentmethod表中金额就是预付款冲完所有账单
                             select b);
            var billQuery = advanceBillQuery as IList<BillRecord> ?? advanceBillQuery.ToList();
            if (billQuery.ToList().Count > 0)
            {
                return true;
            }
            return false;
        }
        //当前账单付款中包含的预付款金额
        private decimal? GetBillsAdvancePaymentAmount(IList<BillRecord> billRecords)
        {
            var advanceBillQuery = (from b in billRecords
                                    join p in _paymentLineItemRepository.Table on b.Id equals p.Bill.Id
                                    join m in _paymentMethodItemRepository.Table on p.Payment.Id equals m.Payment.Id
                                    where b.Status == BillRecord.BillStatusOption.已结账单
                                    && m.PaymentMethod == PaymentMethodOption.预付款
                                    select m).Distinct();
            var paymentMethodItemRecord = advanceBillQuery.FirstOrDefault();
            if (paymentMethodItemRecord != null) return paymentMethodItemRecord.Amount;
            return 0;
        }
        //当前账单付款中非预付款金额
        private decimal GetBillsNonAdvancePaymentAmount(IList<BillRecord> billRecords)
        {
            var advanceBillQuery = (from b in billRecords
                                    join p in _paymentLineItemRepository.Table on b.Id equals p.Bill.Id
                                    join m in _paymentMethodItemRepository.Table on p.Payment.Id equals m.Payment.Id
                                    where b.Status == BillRecord.BillStatusOption.已结账单
                                    && m.PaymentMethod != PaymentMethodOption.预付款
                                    select m).Distinct();
            var paymentMethodItemRecord = advanceBillQuery.ToList().GroupBy(x=>x.Payment.Id).Select(g=>g.Sum(x=>x.Amount)).FirstOrDefault();
            return paymentMethodItemRecord;
          
        }
        /// <summary>
        /// 将数据按照列名输出到Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gridColumns"></param>
        /// <param name="data"></param>
        /// <param name="sheet"></param>
        /// <param name="startRow"></param>
        /// <returns></returns>
        public void FillExcel<T>(string gridColumns, IEnumerable<T> data, ISheet sheet) {
            // Add header lables
            JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
            List<GridColumn> columns = jsonSerialize.Deserialize<List<GridColumn>>(gridColumns);
            var rowIndex = sheet.LastRowNum ==0 ? sheet.LastRowNum : sheet.LastRowNum + 1;
            var row = sheet.CreateRow(rowIndex);
            for (int index = 0; index < columns.Count; index++)
            {
                var column = columns[index];
                row.CreateCell(index).SetCellValue(column.Label);
            }

            rowIndex++;

            //// Add data rows
            foreach (var model in data)
            {
                row = sheet.CreateRow(rowIndex);
                for (int index = 0; index < columns.Count; index++)
                {
                    var propertyName = columns[index].Name;
                    var propertyInfo = model.GetType().GetProperty(propertyName);
                    row.CreateCell(index).SetCellValue(propertyInfo.GetValue(model, null));
                }
                rowIndex++;
            }

            for (int index = 0; index < columns.Count; index++) {
                sheet.AutoSizeColumn(index);
            }
        }

        #region private method

        /// <summary>
        /// 获取费用承担人
        /// </summary>
        /// <param name="billRecord"></param>
        /// <returns></returns>
        private string GetExpenserName(BillRecord billRecord)
        {
            var query = from customer in _customerRepository.Table
                        where customer.Id == billRecord.CustomerId
                        select customer.Name;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取计量单位，如果没有单位的返回空字符串
        /// </summary>
        /// <param name="charge"></param>
        /// <returns></returns>
        private string GetChargeUnit(ChargeItemSettingCommonRecord charge) {
            if (charge.MeterTypeId.HasValue) {
                var meterType = _meterTypeRepository.Table.FirstOrDefault(m => m.Id == charge.MeterTypeId);
                if (meterType != null) {
                    return meterType.Unit;
                }
            }
            return string.Empty;
        }

        #endregion
    }
}