using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Self_Service_MVC.Services;
using Self_Service_MVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Self_Service_MVC.Services
{
    public class UserMemoryService : IUserService
    {
        private readonly List<UserInfo> _userinfo = new List<UserInfo>();

        public UserMemoryService()
        {
            _userinfo.Add(new UserInfo
            {
                ID = 1,
                Username = "Zhang",
                UserPhone = "10086",
            });
            _userinfo.Add(new UserInfo
            {
                ID = 2,
                Username = "Peng",
                UserPhone = "10010"
            });
        }

        public Task AddAsync(UserInfo user)
        {
            var maxId = _userinfo.Max(x => x.ID);
            user.ID = maxId + 1;
            _userinfo.Add(user);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<UserInfo>> GetAllAsync()
        {
            return Task.Run(() => _userinfo.AsEnumerable());
        }

        public Task<UserInfo> GetByIdAsync(int id)
        {
            return Task.Run(() => _userinfo.FirstOrDefault(x => x.ID == id));
        }

        public Task<UserInfo> CheckAccount(string username)
        {
            return Task.Run(() => _userinfo.Find(x => x.Username == username));
        }

        public Task Login(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}


