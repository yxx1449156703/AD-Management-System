using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Self_Service_MVC.Models;


namespace Self_Service_MVC.Services
{
    public interface IUserService
    {
        Task Login(string username, string password);
        Task<IEnumerable<UserInfo>> GetAllAsync();
        Task<UserInfo> GetByIdAsync(int id);
        Task AddAsync(UserInfo user);
        
    }
}
