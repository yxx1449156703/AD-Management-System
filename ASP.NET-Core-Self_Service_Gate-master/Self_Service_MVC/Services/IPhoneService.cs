using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Self_Service_MVC.Services
{
    public interface IPhoneService
    {
        Task<string> SendRequest(string account, string password, string mobile, string content);
    }
}
