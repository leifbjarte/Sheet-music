using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data.Objects;

namespace SBBArkiv
{
    public static class EntitiesFactory
    {
        internal const string DB = "ASP_EDMX_DB_CONTEXT";

        /// <summary>
        /// Get an instance that lives for the life time of the request per user and automatically disposes.
        /// </summary>
        /// <returns>Model</returns>  
        public static MusicArchiveContext AsSingleton()
        {
            HttpContext.Current.Items[DB] = (MusicArchiveContext)HttpContext.Current.Items[DB] ?? new MusicArchiveContext();
            return (MusicArchiveContext)HttpContext.Current.Items[DB];
        }

        /// <summary>
        /// Get an instance that lives for the life time of the Session.
        /// </summary>
        /// <returns>Model</returns>  
        public static MusicArchiveContext AsSession()
        {
            HttpContext.Current.Session[DB] = (MusicArchiveContext)HttpContext.Current.Session[DB] ?? new MusicArchiveContext();
            return (MusicArchiveContext)HttpContext.Current.Session[DB];
        }

        /// <summary>
        /// Get a new instance of the Model. Object must be disposed of.
        /// </summary>
        /// <returns>Model</returns>  
        public static MusicArchiveContext AsNewInstance()
        {
            return new MusicArchiveContext();
        }
    }
}
