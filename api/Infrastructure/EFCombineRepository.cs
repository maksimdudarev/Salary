﻿using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MD.Salary.WebApi.Infrastructure
{
    public class EFCombineRepository : ICombineRepository
    {
        private readonly EmployeeContext _dbContext;

        public EFCombineRepository(EmployeeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Employee> GetEmployeeByIdAsync(long id)
        {
            return _dbContext.Employees.FirstOrDefaultAsync(s => s.ID == id);
        }

        public Task<List<Employee>> EmployeeListBySearhstringAsync(string searchString)
        {
            var items = from i in _dbContext.Employees select i;
            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Name.Contains(searchString));
            }
            return items.ToListAsync();
        }

        public Task AddEmployeeAsync(Employee item)
        {
            _dbContext.Employees.Add(item);
            return _dbContext.SaveChangesAsync();
        }

        public Task UpdateEmployeeAsync(Employee item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            return _dbContext.SaveChangesAsync();
        }

        public Task DeleteEmployeeAsync(Employee item)
        {
            _dbContext.Employees.Remove(item);
            return _dbContext.SaveChangesAsync();
        }

        public Task<User> GetUserByIdAsync(long id)
        {
            return _dbContext.Users.FirstOrDefaultAsync(s => s.ID == id);
        }

        public Task<User> GetUserByNameAsync(string name)
        {
            return _dbContext.Users.FirstOrDefaultAsync(s => s.Name == name);
        }

        public Task<List<User>> UserListAsync()
        {
            return _dbContext.Users.ToListAsync();
        }

        public Task AddUserAsync(User item)
        {
            _dbContext.Users.Add(item);
            return _dbContext.SaveChangesAsync();
        }

        public Task<Token> GetTokenByValueAsync(string value)
        {
            return _dbContext.Tokens.FirstOrDefaultAsync(s => s.Value == value);
        }

        public Task<List<Token>> TokenListAsync()
        {
            return _dbContext.Tokens.ToListAsync();
        }

        public Task AddTokenAsync(Token item)
        {
            _dbContext.Tokens.Add(item);
            return _dbContext.SaveChangesAsync();
        }

        public Task DeleteTokenAsync(Token item)
        {
            _dbContext.Tokens.Remove(item);
            return _dbContext.SaveChangesAsync();
        }
    }
}
