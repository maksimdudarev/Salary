using System.Collections.Generic;
using System.Linq;
using ContactsApi.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MD.Salary.WebApi.Infrastructure;

namespace ContactsApi.Repository
{
    public class EFContactsRepository : IContactsRepository
    {
        EmployeeContext _context;
        public EFContactsRepository(EmployeeContext context)
        {
            _context = context;
        }       

        public async Task Add(Contact item)
        {            
            await _context.Contacts.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Contact>> GetAll()
        {
            return await _context.Contacts.ToListAsync();
        }

        public bool CheckValidUserKey(string reqkey)
        {
            var userkeyList = new List<string>
            {
                "28236d8ec201df516d0f6472d516d72d",
                "38236d8ec201df516d0f6472d516d72c",
                "48236d8ec201df516d0f6472d516d72b"
            };

            if (userkeyList.Contains(reqkey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Contact> Find(string key)
        {
            return await _context.Contacts
                .Where(e => e.MobilePhone.Equals(key))
                .SingleOrDefaultAsync();
        }       

        public async Task Remove(string Id)
        {
            var itemToRemove = await _context.Contacts.SingleOrDefaultAsync(r => r.MobilePhone == Id);
            if (itemToRemove != null)
            {
                _context.Contacts.Remove(itemToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(Contact item)
        {            
            var itemToUpdate = await _context.Contacts.SingleOrDefaultAsync(r => r.MobilePhone == item.MobilePhone);
            if (itemToUpdate != null)
            {
                itemToUpdate.LastName = item.LastName;
                itemToUpdate.IsFamilyMember = item.IsFamilyMember;
                itemToUpdate.MobilePhone = item.MobilePhone;
                await _context.SaveChangesAsync();
            }
        }
    }
}