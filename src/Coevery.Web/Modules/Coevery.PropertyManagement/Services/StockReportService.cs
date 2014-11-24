using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.Data;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.ViewModels;

namespace Coevery.PropertyManagement.Services
{
    public interface IStockReportService : IDependency
    {
        StockSummaryEntity GetStockAmountAndNumber(int materialId, DateTime? beginDate, DateTime? endDate,
            InventoryChangeOperation inventoryChangeOperation, bool isGetBeginData);

        StockSummaryEntity GetBeginingAmountAndNumber(int materialId, DateTime? endDate);
    }

    public class StockReportService : IStockReportService
    {
        private readonly IRepository<InventoryChangeRecord> _inventoryChangeRepository;

        public StockReportService(IRepository<InventoryChangeRecord> inventoryChangeRepository)
        {
            _inventoryChangeRepository = inventoryChangeRepository;
        }

        public StockSummaryEntity GetStockAmountAndNumber(int materialId, DateTime? beginDate, DateTime? endDate,InventoryChangeOperation  inventoryChangeOperation,bool isGetBeginData)
        {
            var stockSummaryEntity = new StockSummaryEntity();
            var query = _inventoryChangeRepository.Table;
            if (beginDate != null)
            {
                query = query.Where(x => x.Date >= beginDate);
            }
            if (endDate != null)
            {
                query = query.Where(x => x.Date <= endDate);
            }
            if (endDate != null && isGetBeginData)
            {
                query = query.Where(x => x.Date < endDate);
            }
            var inventoryChangeRecords = query.ToList();
            var stockInRecordGroup = inventoryChangeRecords
                .Where(x => x.Operation == inventoryChangeOperation && x.MaterialId == materialId)
                .GroupBy(x => x.MaterialId).ToList();
            if (stockInRecordGroup.Count == 0)
            {
                stockSummaryEntity.Number = 0;
                stockSummaryEntity.Amount = 0;
                return stockSummaryEntity;
            }
            foreach (var stockInRecord in stockInRecordGroup)
            {
                stockSummaryEntity.Amount = stockInRecord.Sum(i => i.CostPrice * i.Number);
                stockSummaryEntity.Number = stockInRecord.Sum(i => i.Number);
            }
            return stockSummaryEntity;
        }

       
        //期初  拉通算出所有总金额，总数量，算出单价平均值 ==》 期初数量=所有入库-出库，平均值*期初数量=期初总金额
        public  StockSummaryEntity GetBeginingAmountAndNumber(int materialId, DateTime? endDate)
        {
            var stockSummaryEntity = new StockSummaryEntity();
            var query = _inventoryChangeRepository.Table;
            if (endDate != null)
            {
                query = query.Where(x => x.Date < endDate);
            }
            var inventoryChangeRecords = query.ToList();
            var stockInRecordGroup = inventoryChangeRecords
                .Where(x => x.MaterialId == materialId)
                .GroupBy(x => x.MaterialId).ToList();
            if (stockInRecordGroup.Count == 0)
            {
                stockSummaryEntity.Number = 0;
                stockSummaryEntity.Amount = 0;
                return stockSummaryEntity;
            }
            foreach (var stockInRecord in stockInRecordGroup)
            {
                stockSummaryEntity.Amount = stockInRecord.Sum(i => i.CostPrice * i.Number);
                stockSummaryEntity.Number = stockInRecord.Sum(i => i.Number);
            }
            var averagePrice = stockSummaryEntity.Amount / stockSummaryEntity.Number;
            stockSummaryEntity.Number = GetStockAmountAndNumber(materialId, null, endDate,InventoryChangeOperation.入库, true).Number
                -GetStockAmountAndNumber(materialId, null, endDate,InventoryChangeOperation.出库, true).Number;
            stockSummaryEntity.Amount = averagePrice * stockSummaryEntity.Number;
            return stockSummaryEntity;
        }
    }
}