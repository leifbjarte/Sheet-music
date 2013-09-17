using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SBBArkiv
{
    public partial class MusicParts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MusicPartsGrid.Styles.CommandColumnItem.Paddings.PaddingLeft = Unit.Pixel(3);

            if (Master.GetGroupForUser() != UserGroupType.Administrator)
            {
                MusicPartsGrid.Visible = false;
                lblMessage.Text = "Du har ikke tilgang til denne siden";
                return;
            }
        }

        #region EntityDataSource events

        public void EntityDatasource_ContextCreating(object sender, EntityDataSourceContextCreatingEventArgs e)
        {
            e.Context = EntitiesFactory.AsSingleton();
        }

        public void EntityDatasource_ContextDisposing(object sender, EntityDataSourceContextDisposingEventArgs e)
        {
            e.Cancel = true;
        }

        #endregion EntityDataSource events
    }
}