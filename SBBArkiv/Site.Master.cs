using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Diagnostics;
using System.Web.Security;
using System.Configuration;

namespace SBBArkiv
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        /// <summary>
        /// The location of the pdf files
        /// </summary>
        public string PDF_CONTENT_DIR
        {
            get { return ConfigurationManager.AppSettings["PdfContentDir"]; }
        }

        public string TempUploadDirectory
        {
            get { return Server.MapPath("~/App_Data/UploadTemp"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (GetGroupForUser())
            {
                case UserGroupType.Administrator:
                    break;
                case UserGroupType.Musician:
                    HideMenuItem("MusicPlayer");
                    HideMenuItem("MusicParts");
                    break;
                case UserGroupType.NotLoggedIn:
                    NavigationMenu.Visible = false;
                    break;
            }
        }

        private void HideMenuItem(string value)
        {
            NavigationMenu.Items.Remove(NavigationMenu.Items.OfType<MenuItem>().FirstOrDefault(o => o.Value == value));
        }

        protected void LoginStatus_LoggedOut(object sender, EventArgs e)
        {
            Session.Abandon();
        }

        /// <summary>
        /// Gets the user group type for the logged in user
        /// </summary>
        /// <returns>The user group type</returns>
        public UserGroupType GetGroupForUser()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                User user = Session["UserProfile"] as User;

                if (user != null)
                {
                    return user.UserGroupId != (int)UserGroupType.Musician ? UserGroupType.Administrator : UserGroupType.Musician;
                }
                else
                {
                    Session.Abandon();
                    FormsAuthentication.SignOut();
                    return UserGroupType.NotLoggedIn;
                }
            }
            else
            {
                return UserGroupType.NotLoggedIn;
            }
        }
    }
}
