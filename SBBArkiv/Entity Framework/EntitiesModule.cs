using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data.Objects;

namespace SBBArkiv
{
    /// <summary>
    /// Entity Module used to control an Entities DB context over the lifetime of a request per user.
    /// </summary>
    public class EntitiesModule : IHttpModule
    {
        private const string DB = EntitiesFactory.DB;


        void context_EndRequest(object sender, EventArgs e)
        {
            Dispose();
        }


        #region IHttpModule Members

        public void Dispose()
        {
            if (HttpContext.Current != null && HttpContext.Current.Items[DB] != null)
            {
                ObjectContext entitiesContext = (ObjectContext)HttpContext.Current.Items[DB];
                entitiesContext.Connection.Close();
                entitiesContext.Connection.Dispose();
                entitiesContext.Dispose();
            }
        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += new EventHandler(context_EndRequest);
        }

        #endregion

    }
}
