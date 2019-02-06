using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;

  // install-package Microsoft.AspNet.Identity.Owin
    // install-package Microsoft.AspNet.Identity.EntityFramework
    // install-package Microsoft.Owin.Host.SystemWeb
    // install-package Microsoft.Owin.Security.Cookies
[assembly: OwinStartup(typeof(Identity_MVC.Startup))]

namespace Identity_MVC
{ 
    // We can add this class by adding OWINS startup class 
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            // 1. 
            app.CreatePerOwinContext(() => new IdentityDbContext(connectionString));
            app.CreatePerOwinContext<UserStore<IdentityUser>>((opt, cont) =>
                new UserStore<IdentityUser>(cont.Get<IdentityDbContext>()));
            // 2. 
            app.CreatePerOwinContext<UserManager<IdentityUser>>((opt, cont) => new UserManager<IdentityUser>(cont.Get<UserStore<IdentityUser>>()));

            // 3. 
            app.CreatePerOwinContext<SignInManager<IdentityUser,string>>
                    ((opt, cont) => new SignInManager<IdentityUser, string>(cont.Get<UserManager<IdentityUser>>(), cont.Authentication));

            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie

            });


            
        }
    }
}
