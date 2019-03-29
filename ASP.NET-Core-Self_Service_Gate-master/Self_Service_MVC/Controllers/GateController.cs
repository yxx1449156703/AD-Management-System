using System.Threading.Tasks;
using Self_Service_MVC.Services;
using Microsoft.AspNetCore.Mvc;
using Self_Service_MVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Session;

namespace Self_Service_MVC.Services
{
    public class GateController : Controller
    {
        private readonly IUserService _userService;
        private readonly PhoneValidator _validator;

        // 构造函数 依赖注入
        public GateController(IUserService userService, PhoneValidator pv)
        {
            _userService = userService;
            _validator = pv;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Forgot()
        {
            return View();
        }

        public IActionResult ConfirmByEmail()
        {
            return View();
        }

        public IActionResult ConfirmByPhone()
        {
            return View();
        }

        public IActionResult Navagation()
        {
            return View();
        }

        public IActionResult CheckAccount()
        {
            return RedirectToAction("ResetPage", "Reset");
        }
        
    }
}