using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Self_Service_MVC.Services;

namespace Self_Service_MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<IUserService, UserMemoryService>();
            services.AddSingleton<IMailService, MailMemoryService>();
            CreatePhoneValidator(services);
        }

        private void CreatePhoneValidator(IServiceCollection services)
        {
            Hashtable segment = new Hashtable();
            var coll = Configuration.GetSection("phone-segment").GetChildren();
            foreach (var prefix in coll)
            {
                if (string.IsNullOrEmpty(prefix.Value))
                    continue;
                foreach (var s in prefix.Value.Split(','))
                    segment[s] = s;
            }
            var pv = new PhoneValidator(segment);
            services.AddSingleton<PhoneValidator>(pv);
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Gate}/{action=Login}/{id?}");
            });
        }
    }
}
