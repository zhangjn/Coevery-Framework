using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.Data;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.Services
{
    public interface IInventoryService : IDependency
    {
        void IncreaseInventory(InventoryInfo info, int number);
        void IncreaseInventory(int id, int number);
        void DecreaseInventory(int id, int number, decimal stockOutPrice);
    }

    public class InventoryService : IInventoryService
    {
        private readonly IRepository<InventoryRecord> _inventoryRepository;

        public InventoryService(IRepository<InventoryRecord> inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public void IncreaseInventory(InventoryInfo info, int number)
        {
            if (number <= 0)
            {
                return;
            }
            var inventory = _inventoryRepository.Table
                .FirstOrDefault(x => x.MaterialId == info.MaterialId);

            if (inventory == null)
            {
                _inventoryRepository.Create(new InventoryRecord
                {
                    MaterialId = info.MaterialId,
                    CostPrice = info.CostPrice,
                    Number = number,
                    Amount = info.CostPrice * number
                });
            }
            else
            {
                inventory.Number += number;
                inventory.Amount += info.CostPrice*number;
                inventory.CostPrice = inventory.Amount / inventory.Number;
            }
        }

        public void IncreaseInventory(int id, int number)
        {
            if (number <= 0)
            {
                return;
            }
            var inventory = _inventoryRepository.Get(id);
            if (inventory != null)
            {
                inventory.Number += number;
            }
        }

        public void DecreaseInventory(int id, int number,decimal stockOutPrice)
        {
            if (number <= 0)
            {
                return;
            }
            var inventory = _inventoryRepository.Table
                .FirstOrDefault(x => x.MaterialId == id); 
            if (inventory == null || inventory.Number < number)
            {
                return;
            }
            inventory.Number -= number;
            inventory.Amount -= stockOutPrice*number;
        }
    }

    public class InventoryInfo
    {
        public int MaterialId { get; set; }
        public decimal CostPrice { get; set; }

    }
}