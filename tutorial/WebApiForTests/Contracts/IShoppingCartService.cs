using System;
using System.Collections.Generic;
using WebApiForTests.Model;

namespace WebApiForTests.Contracts
{
    public interface IShoppingCartService
    {
        IEnumerable<ShoppingItem> GetAllItems();
        ShoppingItem Add(ShoppingItem newItem);
        ShoppingItem GetById(Guid id);
        void Remove(Guid id);
    }
}
