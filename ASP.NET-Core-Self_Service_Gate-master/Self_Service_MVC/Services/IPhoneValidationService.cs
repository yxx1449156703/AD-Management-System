using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Self_Service_MVC.Services
{
    public interface IPhoneValidationService
    {
        bool IsPhone(ref string tel);
    }
}
