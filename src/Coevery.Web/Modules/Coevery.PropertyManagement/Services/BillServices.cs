using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.Data;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.Utility;
using Coevery.PropertyManagement.ViewModels;

namespace Coevery.PropertyManagement.Services {
    public class BillServices : IBillServices {
        private readonly IRepository<HouseMeterReadingPartRecord> _houseMeterReadingRepository;
        private readonly IRepository<BillRecord> _repositoryBill;

        public BillServices(IRepository<HouseMeterReadingPartRecord> houseMeterReadingRepository,
            IRepository<BillRecord> repositoryBill) {
            _houseMeterReadingRepository = houseMeterReadingRepository;
            _repositoryBill = repositoryBill;
        }

        public IEnumerable<BillListViewModel> GenerateBillListViewModels(IList<ContractHouseRecord> contractHouseRecords) {
            var list = new List<BillListViewModel>();
            foreach (var house in contractHouseRecords) {
                var contract = house.Contract;

                #region 合同里面的收费项目

                foreach (var item in contract.ChargeItems) {
                    var chargeItemEntry = new ChargeItemEntry {
                        BeginDate = item.BeginDate,
                        ChargeItemSetting = item,
                        EndDate = item.EndDate,
                        ExpenserOption = (ExpenserOption) item.ExpenserOption,
                        Owner = house.House.Owner,
                        Renter = contract.Renter
                    };
                    GenerateBillListViewModels(contract, house.House, chargeItemEntry, list);
                }

                #endregion

                #region 房间里面的收费项目

                foreach (var item in house.House.ChargeItems) {
                    var chargeItemEntry = new ChargeItemEntry {
                        BeginDate = item.BeginDate,
                        ChargeItemSetting = item,
                        EndDate = item.EndDate,
                        ExpenserOption = (ExpenserOption)item.ExpenserOption,
                        Owner = house.House.Owner,
                        Renter = contract.Renter
                    };
                    GenerateBillListViewModels(contract, house.House, chargeItemEntry, list);
                }

                #endregion
            }
            return list;
        }

        private void GenerateBillListViewModels(
            ContractPartRecord record,
            HousePartRecord house,
            ChargeItemEntry chargeItem,
            List<BillListViewModel> list) {
            //获取合同收费项目开始日期与结束日期


            //去Bill表中查询已缴或已出账单日期时间到哪里了
            var latestBill = _repositoryBill.Table
                .Where(c => c.House.Id == house.Id && c.Contract.Id == record.Id && c.ChargeItem == chargeItem.ChargeItemSetting)
                .OrderByDescending(m => m.EndDate).FirstOrDefault();

            if (chargeItem.ChargeItemSetting.ItemCategory.HasValue) {
                if (chargeItem.ChargeItemSetting.ItemCategory == ItemCategoryOption.临时性收费项目) {
                    //只收一次，只要曾经收过就不再收取
                    if (latestBill != null) return;
                    DateTime beginDate = record.BeginDate;
                    DateTime endDate = record.EndDate;
                    var model = CreateBillListViewModel(record, house, chargeItem, beginDate, endDate);
                    CaculateChargeItemAmount(record, house, chargeItem, model, ItemCategoryOption.临时性收费项目, DateTime.Now);
                    list.Add(model);
                }
                else {
                    DateTime beginDate = chargeItem.BeginDate;
                    if (latestBill != null) beginDate = latestBill.EndDate.AddDays(1); //找到已结账的最后日期
                    if (record.BeginDate > beginDate) beginDate = record.BeginDate; //比合同时间还早，就默认为合同开始时间

                    DateTime maxEndDate = chargeItem.EndDate ?? DateTime.MaxValue; //默认结束时间为当前时间
                    if (record.EndDate < maxEndDate) maxEndDate = record.EndDate; //比合同时间还晚，默认为合同结束时间

                    int period;
                    if (chargeItem.ChargeItemSetting.ItemCategory == ItemCategoryOption.抄表类收费项目) {
                        period = 1;
                    }
                    else {
                        period = (int) ((chargeItem.ChargeItemSetting.ChargingPeriod) ?? ChargingPeriodOption.每1个月收一次);
                    }

                    DateTime periodBeginDate = beginDate; //开始周期月时间

                    while (IsValidPeriod(chargeItem.ChargeItemSetting.DefaultChargingPeriod, period, periodBeginDate)
                           && periodBeginDate < maxEndDate) {
                        DateTime periodEndDate = periodBeginDate.AddMonths(period).AddDays(-1);
                        if (periodEndDate > maxEndDate) periodEndDate = maxEndDate;
                        var model = CreateBillListViewModel(record, house, chargeItem, periodBeginDate, periodEndDate);
                        CaculateChargeItemAmount(record, house, chargeItem, model, chargeItem.ChargeItemSetting.ItemCategory.Value, periodBeginDate);
                        list.Add(model);
                        periodBeginDate = periodEndDate.AddDays(1);
                    }
                }
            }
        }

        private bool IsValidPeriod(DefaultChargingPeriodOption? option, int duration, DateTime beginDate) {
            option = option ?? DefaultChargingPeriodOption.当期收当期;
            var currentPeriodBeginDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var currentPeriodEndDate = currentPeriodBeginDate.AddMonths(duration);
            switch (option) {
                case DefaultChargingPeriodOption.当期收上期:
                    return beginDate < currentPeriodBeginDate;
                case DefaultChargingPeriodOption.当期收当期:
                    return beginDate < currentPeriodEndDate;
                case DefaultChargingPeriodOption.当期收下期:
                    var nextPeriodBeginDate = currentPeriodEndDate;
                    var nextPeriodEndDate = nextPeriodBeginDate.AddMonths(duration);
                    return beginDate < nextPeriodEndDate;
                default:
                    return false;
            }
        }

        private static BillListViewModel CreateBillListViewModel(ContractPartRecord record, HousePartRecord house, ChargeItemEntry chargeItem, DateTime beginDate, DateTime endDate) {
            var model = new BillListViewModel {
                ContractId = record.Id,
                HouseId = house.Id,
                CustomerId = chargeItem.Expenser.Id,
                ChargeItemSettingId = chargeItem.ChargeItemSetting.Id, //存合同或房间chargeItem的Id(已经转换过了)
                ContractNumber = record.Number,
                HouseNumber = house.HouseNumber,
                ApartmentName = house.Apartment.Name,
                BuildingName = house.Building.Name,
                ChargeItemName = chargeItem.ChargeItemSetting.Name,
                ChargeItemSettingName = chargeItem.ChargeItemSetting.Name,
                UnitPrice = chargeItem.ChargeItemSetting.UnitPrice,
                BeginDate = beginDate,
                EndDate = endDate,
                OfficerName = house.Officer.UserName
            };
            return model;
        }

        private void CaculateChargeItemAmount(ContractPartRecord contract, HousePartRecord house, ChargeItemEntry chargeItem,
            BillListViewModel model, ItemCategoryOption itemCategoryOption, DateTime beginDate) {

            switch (chargeItem.ChargeItemSetting.CalculationMethod) {
                case CalculationMethodOption.指定金额:
                    model.Amount = chargeItem.ChargeItemSetting.Money;
                    break;
                case CalculationMethodOption.单价数量: {
                    if (itemCategoryOption == ItemCategoryOption.临时性收费项目) {
                        var meteringMode = chargeItem.ChargeItemSetting.MeteringMode;
                        if (meteringMode == MeteringModeOption.建筑面积) {
                            model.Amount = chargeItem.ChargeItemSetting.UnitPrice*(decimal) (house.BuildingArea ?? 0);
                        }
                        else //套内面积
                        {
                            model.Amount = chargeItem.ChargeItemSetting.UnitPrice*(decimal) (house.InsideArea ?? 0);
                        }
                    }

                    if (itemCategoryOption == ItemCategoryOption.周期性收费项目) {
                        var meteringMode = chargeItem.ChargeItemSetting.MeteringMode;
                        if (meteringMode == MeteringModeOption.建筑面积) {
                            model.Amount = chargeItem.ChargeItemSetting.UnitPrice*(decimal) (house.BuildingArea ?? 0);
                        }
                        else //套内面积
                        {
                            model.Amount = chargeItem.ChargeItemSetting.UnitPrice*(decimal) (house.InsideArea ?? 0);
                        }
                    }
                    if (itemCategoryOption == ItemCategoryOption.抄表类收费项目) {
                        if (chargeItem.ChargeItemSetting.MeterTypeId.HasValue) {
                            var readings = _houseMeterReadingRepository.Table.Where(x => x.HouseMeter.House.Id == house.Id).ToList();
                            var houseMeter = house.MeterTypeItems.FirstOrDefault(x => x.MeterType.Id == chargeItem.ChargeItemSetting.MeterTypeId);
                            if (houseMeter != null) {
                                var houseMeterReadings = readings.Where(x => x.HouseMeter == houseMeter).ToList();
                                if (houseMeterReadings.Any()) {
                                    var previousReadingDate = beginDate.AddMonths(-1).Date;
                                    var previous = houseMeterReadings.FirstOrDefault(c => c.Year == previousReadingDate.Year && c.Month == previousReadingDate.Month);
                                    var current = houseMeterReadings.FirstOrDefault(c => c.Year == beginDate.Year && c.Month == beginDate.Month);
                                    model.LastReading = previous != null ? previous.MeterData : null;
                                    model.CurrentReading = current != null ? current.MeterData : null;
                                    if (model.Quantity.HasValue && model.UnitPrice.HasValue) {
                                        model.Amount = model.UnitPrice*(decimal) model.Quantity;
                                    }
                                }
                            }
                        }
                    }

                    break;
                }

                case CalculationMethodOption.自定义公式:
                    //todo
                    model.Amount = (decimal?) CaculateFormula(chargeItem.ChargeItemSetting.CustomFormula, house, chargeItem.ChargeItemSetting, contract);
                    break;
            }

        }


        /// <summary>
        /// 计算自定义公式
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="house"></param>
        /// <param name="chargeItemSetting"></param>
        /// <returns></returns>
        private object CaculateFormula(string formula, HousePartRecord house, ChargeItemSettingCommonRecord chargeItemSetting, ContractPartRecord contract) {

            var parameters = new List<ParameterEntity>();
            var contractHouseRecord = contract.Houses.FirstOrDefault(x => x.House.Id == house.Id);
            if (contractHouseRecord != null) {
                var amount = chargeItemSetting.MeteringMode == MeteringModeOption.建筑面积 ? house.BuildingArea : house.InsideArea;
                if (amount != null) {
                    parameters.Add(new ParameterEntity {
                        ParaName = "数量",
                        ParaType = typeof (decimal),
                        ParaValue = (decimal) amount
                    });
                }
            }

            parameters.Add(new ParameterEntity {
                ParaName = "单价",
                ParaType = typeof (decimal),
                ParaValue = chargeItemSetting.UnitPrice
            });


            try {
                var result = Evaluator.EvaluateToObject(formula, parameters);
                return result;
            }
            catch (Exception ex) {
                return decimal.Zero;
            }

        }

        private class ChargeItemEntry {
            public DateTime BeginDate { get; set; }
            public DateTime? EndDate { get; set; }


            // public ChargeItemSettingPartRecord ChargeItemSetting { get; set; }
            public ChargeItemSettingCommonRecord ChargeItemSetting { get; set; }
            public CustomerPartRecord Owner { get; set; }
            public CustomerPartRecord Renter { get; set; }

            public CustomerPartRecord Expenser {
                get { return ExpenserOption == ExpenserOption.业主 ? Owner : Renter; }
            }

            public ExpenserOption ExpenserOption { get; set; }

        }
    }
}