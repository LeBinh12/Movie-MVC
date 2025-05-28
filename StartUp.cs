using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
[assembly: OwinStartup(typeof(WbeMovieUser.StartUp))]

namespace WbeMovieUser
{
    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}