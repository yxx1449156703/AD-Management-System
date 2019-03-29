using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.IdentityModel.Protocols;
using System.Collections.Generic;

namespace Self_Service_MVC.Services
{
    public class PhoneMemoryService: IPhoneService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<string> SendRequest(string account, string password, string mobile, string content)
        {
            var values = new Dictionary<string, string>
            {
                {"account", account },
                {"password", password },
                {"mobile", mobile },
                {"content", content }
            };
            var httpcontent = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("http://106.ihuyi.com/webservice/sms.php?method=Submit", httpcontent);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }

}
