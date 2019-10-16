﻿using MD.Salary.WebApi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MD.Salary.WebApi.Core.Interfaces
{
    public interface ICombineRepository
    {
        Task<Employee> GetEmployeeByIdAsync(long id);
        Task<List<Employee>> EmployeeListBySearhstringAsync(string searchString = "");
        Task AddEmployeeAsync(Employee item);
        Task UpdateEmployeeAsync(Employee item);
        Task DeleteEmployeeAsync(Employee item);
        Task<User> GetUserByIdAsync(long id);
        Task<User> GetUserByNameAsync(string name);
        Task<List<User>> UserListAsync();
        Task AddUserAsync(User item);
        Task<Token> GetTokenByValueAsync(string value);
        Task<List<Token>> TokenListAsync();
        Task AddTokenAsync(Token item);
        Task DeleteTokenAsync(Token item);
    }
}
