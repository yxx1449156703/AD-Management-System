using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Self_Service_MVC.Models;
using Self_Service_MVC.Services;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Self_Service_MVC.Services
{
    public class ResetController : Controller
    {

        private readonly PhoneValidator _validator;
        PhoneMemoryService sendphone = new PhoneMemoryService();
        private string phone;
        private int usercode;
        private int officalcode;
        private Task<string> returnData;

        // 构造函数 依赖注入
        public ResetController(PhoneValidator pv)
        {
            _validator = pv;
        }

        public IActionResult Reset()
        {
            return View();
        }

        public IActionResult ResetPage(UserInfo user)
        {
            usercode = user.MobileCode; //用户填入的code
            officalcode = user.OfficalCode;
            if (usercode == officalcode)
            {
                return View();
            }
            else
            {
                return RedirectToAction("ConfirmByPhone", "Gate");
            }
        }


        public IActionResult InputPhoneCode(UserInfo user)
        {
            String product = "Dysmsapi";//短信API产品名称（短信产品名固定，无需修改）
                                        //String domain = "dysmsapi.aliyuncs.com";//短信API产品域名（接口地址固定，无需修改）
            String domain = "dysmsapi.aliyuncs.com";
            String accessKeyId = "**********";//你的accessKeyId，参考本文档步骤2
            String accessKeySecret = "******************";//你的accessKeySecret，参考本文档步骤2

            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            //IAcsClient client = new DefaultAcsClient(profile);
            // SingleSendSmsRequest request = new SingleSendSmsRequest();
            //初始化ascClient,暂时不支持多region（请勿修改）
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            Random rad = new Random();
            int mobile_code = rad.Next(100000, 1000000);
            try
            {
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式，发送国际/港澳台消息时，接收号码格式为00+国际区号+号码，如“0085200000000”
                request.PhoneNumbers = user.UserPhone;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = "****";
                //必填:短信模板-可在短信控制台中找到，发送国际/港澳台消息时，请使用国际/港澳台短信模版
                request.TemplateCode = "*********";
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                //request.TemplateParam = "{\"name\":\"Tom\"， \"code\":\"123\"}";
                //request.TemplateParam = "{\"code\":\"123\"}";
                request.TemplateParam = "{\"code\":\"" + mobile_code.ToString() + "\"}";
                //request.TemplateParam = "您正在申请注册，验证码为：${code}，5分钟内有效！";
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                request.OutId = "123";
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
                //System.Console.WriteLine(sendSmsResponse.Message);
            }
            catch (ServerException e)
            {
                string result = e.Message;
                //System.Console.WriteLine("Hello World!");
            }
            catch (ClientException e)
            {
                string result = e.Message;
                //System.Console.WriteLine("Hello World!");
            }
            ViewData["OfficalCode"] = mobile_code;
            return View();
        }


        public IActionResult InputEmailCode(UserInfo user)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = "";
            if (1 == 1) { str += "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            for (int i = 0; i < 6; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }

            string[] sArray = user.UserEmail.Split('@');
            ViewData["email"] = user.UserEmail.Substring(0, 4) + "****@"+ sArray[1];
            string senderServerIp = "smtp.163.com";             //使用163代理邮箱服务器（也可是使用qq的代理邮箱服务器，但需要与具体邮箱对相应）
            string toMailAddress = user.UserEmail;              //要发送对象的邮箱
            string fromMailAddress = "**********";     //你的邮箱
            string subjectInfo = "验证码";                      //主题
            string bodyInfo = "<p>" + s + "</p>";               //以Html格式发送的邮件
            string mailUsername = "**********";        //登录邮箱的用户名
            string mailPassword = "******";                   //对应的登录邮箱的第三方密码（你的邮箱不论是163还是qq邮箱，都需要自行开通stmp服务）
            string mailPort = "***";                             //发送邮箱的端口号
                                                                //string attachPath = "E:\\123123.txt; E:\\haha.pdf";

            //创建发送邮箱的对象
            MailMemoryService myEmail = new MailMemoryService(senderServerIp, toMailAddress, fromMailAddress, subjectInfo, bodyInfo, mailUsername, mailPassword, mailPort, false, false);

            //添加附件
            //email.AddAttachments(attachPath);
            if (myEmail.Send())
            {
                return View();
            }
            else
            {
                return Content("<script>alert('邮件发送失败!')</script>");
            }
        }

    }
}