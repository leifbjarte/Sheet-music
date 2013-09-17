using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Data.Linq;
using DevExpress.Web.ASPxGridLookup;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using System.Data.Objects.DataClasses;

namespace SBBArkiv
{
    public partial class MusicPlayer : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UsersGrid.Styles.CommandColumnItem.Paddings.PaddingLeft = Unit.Pixel(3);

            if (Master.GetGroupForUser() != UserGroupType.Administrator)
            {
                UsersGrid.Visible = false;
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

        protected void PartsLookup_Load(object sender, EventArgs e)
        {
            ASPxGridLookup lookup = sender as ASPxGridLookup;
            lookup.GridView.Width = Unit.Percentage(100);
            lookup.DataSource = LookupRetriever.GetPartLookup();
        }

        protected void PartsLookup_DataBound(object sender, EventArgs e)
        {
            if (UsersGrid.IsNewRowEditing) //do not set current selections for new row
            {
                return;
            }

            EntityCollection<MusicPart> parts = UsersGrid.GetRowValues(UsersGrid.EditingRowVisibleIndex, "MusicParts") as EntityCollection<MusicPart>;
            ASPxGridLookup lookup = sender as ASPxGridLookup;
            lookup.GridView.Selection.UnselectAll();

            foreach (MusicPart part in parts)
            {
                lookup.GridView.Selection.SelectRowByKey(part.Id);
            }
        }

        #region Users grid events

        protected void UsersGrid_Init(object sender, EventArgs e)
        {
            GridViewDataComboBoxColumn userGroup = UsersGrid.Columns["UserGroupId"] as GridViewDataComboBoxColumn;
            userGroup.PropertiesComboBox.DataSource = LookupRetriever.GetUserGroupsLookup();
        }

        protected void UsersGrid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName != "MusicParts_Unbound")
            {
                return;
            }

            User currentRow = UsersGrid.GetRow(e.VisibleRowIndex) as User;
            string parts = string.Empty;

            foreach (MusicPart part in currentRow.MusicParts)
            {
                parts += part.PartName + ", ";
            }

            if (parts.Contains(","))
            {
                parts = parts.Remove(parts.LastIndexOf(","));
            }

            e.DisplayText = parts;
        }

        protected void UsersGrid_RowUpdated(object sender, ASPxDataUpdatedEventArgs e)
        {
            ASPxGridLookup lookup = UsersGrid.FindEditRowCellTemplateControl((GridViewDataColumn)UsersGrid.Columns["MusicParts_Unbound"], "partsLookup") as ASPxGridLookup;
            List<int> selectedPartIds = lookup.GridView.GetSelectedFieldValues("Id").OfType<int>().ToList();
            int userId = (int)e.Keys[0];

            SetPartsForUser(selectedPartIds, userId, lookup.DataSource as List<MusicPart>);
        }

        protected void UsersGrid_RowInserted(object sender, ASPxDataInsertedEventArgs e)
        {
            ASPxGridLookup lookup = UsersGrid.FindEditRowCellTemplateControl((GridViewDataColumn)UsersGrid.Columns["MusicParts_Unbound"], "partsLookup") as ASPxGridLookup;
            List<int> selectedPartIds = lookup.GridView.GetSelectedFieldValues("Id").OfType<int>().ToList();
            List<int> matches = null;

            string name = e.NewValues["Name"].ToString();
            string userName = e.NewValues["UserName"] != null ? e.NewValues["UserName"].ToString() : null;
            string email = e.NewValues["Email"].ToString();

            //attempt to retrieve newly inserted user matching all columns
            MusicArchiveContext ctx = EntitiesFactory.AsSingleton();
            IQueryable<int> query = from u in ctx.Users
                                    where u.Name == name
                                    where u.UserName == userName
                                    where u.Email == email
                                    orderby u.Id
                                    select u.Id;

            matches = query.ToList();

            if (matches == null || matches.Count == 0)
            {
                return;
            }

            SetPartsForUser(selectedPartIds, matches.Max(), lookup.DataSource as List<MusicPart>);
        }

        protected void UsersGrid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            MusicArchiveContext ctx = EntitiesFactory.AsSingleton();
            int userId = (int)e.Keys[0];
            User beingDeleted = ctx.Users.FirstOrDefault(o => o.Id == userId );
            beingDeleted.MusicParts.Clear();
            ctx.SaveChanges();
        }

        #endregion Users grid events

        private void SetPartsForUser(List<int> partIds, int userId, List<MusicPart> parts)
        {
            MusicArchiveContext ctx = EntitiesFactory.AsSingleton();

            User user = ctx.Users.Include("MusicParts").FirstOrDefault(o => o.Id == userId);
            user.MusicParts.Clear();

            foreach (int selection in partIds)
            {
                user.MusicParts.Add(parts.FirstOrDefault(o => o.Id == selection));
            }

            ctx.SaveChanges();

        }
    }
}