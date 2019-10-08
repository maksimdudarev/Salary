﻿//https://www.mithunvp.com/write-custom-asp-net-core-middleware-web-api/
using ContactsApi.Models;
using System.Collections.Generic;

namespace ContactsApi.Repository
{
    public interface IContactsRepository
    {
        void Add(Contacts item);
        IEnumerable<Contacts> GetAll();
        Contacts Find(string key);
        void Remove(string Id);
        void Update(Contacts item);

        bool CheckValidUserKey(string reqkey);
    }
}