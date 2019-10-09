using ContactsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactsApi.Repository
{
    public interface IContactsRepository
    {
        Task Add(Contact item);
        Task<IEnumerable<Contact>> GetAll();
        Task<Contact> Find(string key);
        Task Remove(string Id);
        Task Update(Contact item);

        bool CheckValidUserKey(string reqkey);
    }
}
