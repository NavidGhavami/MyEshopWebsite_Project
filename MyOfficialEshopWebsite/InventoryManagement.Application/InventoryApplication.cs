﻿using System.Collections.Generic;
using _0_Framework.Application;
using InventoryManagement.Application.Contract;
using InventoryManagement.Domain.InventoryAgg;

namespace InventoryManagement.Application
{
    public class InventoryApplication : IInventoryApplication
    {
        private readonly IInventoryRepository _inventoryRepository;
        // private readonly IAuthHelper _authHelper;

        public InventoryApplication(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
            //_authHelper = authHelper;
        }

        public OperationResult Create(CreateInventory command)
        {
            var operation = new OperationResult();
            if (_inventoryRepository.Exist(x => x.ProductId == command.ProductId))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var inventory = new Inventory(command.ProductId, command.UnitPrice);
            _inventoryRepository.Create(inventory);
            _inventoryRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Edit(EditInventory command)
        {
            var operation = new OperationResult();
            var inventory = _inventoryRepository.Get(command.Id);
            if (inventory == null)
            {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }

            if (_inventoryRepository.Exist(x => x.ProductId == command.ProductId && x.Id != command.Id))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }
            inventory.Edit(command.ProductId, command.UnitPrice);

            _inventoryRepository.SaveChanges();

            return operation.Succedded();

        }
        public EditInventory GetDetails(long id)
        {
            return _inventoryRepository.GetDetails(id);
        }

        public List<InventoryViewModel> Search(InventorySearchModel searchModel)
        {
            return _inventoryRepository.Search(searchModel);
        }

        public List<InventoryOperationViewModel> GetOperationLog(long inventoryId)
        {
            return _inventoryRepository.GetOperationLog(inventoryId);
        }

        public OperationResult Increase(IncreaseInventory command)
        {
            var operation = new OperationResult();
            var inventory = _inventoryRepository.Get(command.InventoryId);

            if (inventory == null)
            {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }

            inventory.Increase(command.Count, command.Description);

            _inventoryRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Decrease(DecreaseInventory command)
        {
            var operation = new OperationResult();
            var inventory = _inventoryRepository.Get(command.InventoryId);

            if (inventory == null)
            {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }

            //var operatorId = _authHelper.CurrentAccountId();
            inventory.Decrease(command.Count, command.Description, 0);

            _inventoryRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Decrease(List<DecreaseInventory> command)
        {
            var operation = new OperationResult();
            //var operatorId = _authHelper.CurrentAccountId();
            foreach (var item in command)
            {
                var inventory = _inventoryRepository.GetBy(item.ProductId);
                inventory.Decrease(item.Count, item.Description, item.OrderId);
            }

            _inventoryRepository.SaveChanges();

            return operation.Succedded();

        }

    }
}
