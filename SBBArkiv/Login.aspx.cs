using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SBBArkiv
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/SheetMusicPage.aspx");
            }

            SbbLogin.Focus();
        }

        protected void SbbLogin_LoggedIn(object sender, EventArgs e)
        {
            MusicArchiveContext ctx = EntitiesFactory.AsSingleton();

            User user = ctx.Users.Include("UserGroup").FirstOrDefault(o => o.UserName == SbbLogin.UserName);
            Session["UserProfile"] = user;
        }
    }
}