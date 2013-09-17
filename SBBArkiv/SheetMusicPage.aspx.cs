using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.UI;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxUploadControl;
using DevExpress.Web.Data;
using Ionic.Zip;
using System.Web;
using System.Threading;
using System.Globalization;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallback;
using DevExpress.Web.ASPxClasses;
using System.IO.Compression;
using System.Collections.Generic;

namespace SBBArkiv
{
    public partial class SheetMusicPage : Page
    {
        private List<MusicPart> parts;

        protected void Page_Load(object sender, EventArgs e)
        {
            SheetMusicGrid.Styles.CommandColumnItem.Paddings.PaddingLeft = Unit.Pixel(3);

            switch (Master.GetGroupForUser())
            {
                case UserGroupType.Administrator:
                    break;
                case UserGroupType.Musician:
                    SheetMusicGrid.Columns["SendToAll"].Visible = false;
                    SheetMusicGrid.Columns["Download"].Visible = false;
                    HideCommandButtons(SheetMusicGrid);
                    break;
                case UserGroupType.NotLoggedIn:
                    Response.Redirect("~/Login.aspx");
                    break;
            }
        }

        protected void Control_Load(object sender, EventArgs e)
        {
            Control controlToHide = sender as Control;

            if (Master.GetGroupForUser() != UserGroupType.Administrator)
            {
                controlToHide.Visible = false;
            }
        }

        protected void Grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Editor.GetType() == typeof(ASPxComboBox))
            {
                (e.Editor as ASPxComboBox).DataBind();
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

        #region SheetMusicGrid events

        protected void SheetMusicGrid_Init(object sender, EventArgs e)
        {
            GridViewDataComboBoxColumn categoryColumn = SheetMusicGrid.Columns["SheetMusicCategoryId"] as GridViewDataComboBoxColumn;
            categoryColumn.PropertiesComboBox.DataSource = LookupRetriever.GetCategoryLookup();
        }

        protected void SheetMusicGrid_StartRowEditing(object sender, ASPxStartRowEditingEventArgs e)
        {
            (sender as ASPxGridView).DetailRows.CollapseAllRows();
        }

        protected void SheetMusicGrid_InitNewRow(object sender, ASPxDataInitNewRowEventArgs e)
        {
            (sender as ASPxGridView).DetailRows.CollapseAllRows();
        }

        protected void SheetMusicGrid_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            int musicSheetId = (SheetMusicGrid.GetRow(e.VisibleIndex) as SheetMusic).Id;
            Task.Factory.StartNew(() => SendEmailsForMusicSheet(musicSheetId)); //send email in separate thread.
        }

        protected void SheetMusicCallback_Callback(object sender, CallbackEventArgs e)
        {
            string[] parms = e.Parameter.Split(',');
            string action = parms[0];

            if (action == "SendSheetAsZip")
            {
                ASPxWebControl.RedirectOnCallback("FileDownload.aspx?action=SendSheetAsZip&sheetId=" + parms[1]);
            }
            else if (action == "DownloadSinglePdf")
            {
                ASPxWebControl.RedirectOnCallback("FileDownload.aspx?action=DownloadSinglePdf&sheetPartId=" + parms[1]);
            }
        }

        protected void SheetMusicGrid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            int key = (int)e.Keys[0];

            MusicArchiveContext context = EntitiesFactory.AsSingleton();
            SheetMusic set = context.SheetMusic.Include("SheetMusicParts").FirstOrDefault(o => o.Id == key);

            foreach (SheetMusicPart musicSheetPart in set.SheetMusicParts.ToList())
            {
                context.DeleteObject(musicSheetPart);
            }

            context.DeleteObject(set);
            context.SaveChanges();

            File.Delete(set.ArchiveFileName);

            e.Cancel = true;
        }

        #endregion SheetMusicGrid events

        #region Grid export

        protected void ibtExcelExport_Click(object sender, EventArgs e)
        {
            gridExporter.FileName = "SBB_Arkivliste_" + DateTime.Now.Date.ToShortDateString().Replace(".", "");
            gridExporter.WriteXlsxToResponse();
        }

        protected void ibtPdfExport_Click(object sender, EventArgs e)
        {
            gridExporter.FileName = "SBB_Arkivliste_" + DateTime.Now.Date.ToShortDateString().Replace(".", "");
            gridExporter.WritePdfToResponse();
        }

        #endregion Grid export

        protected void FileUploader_FilesUploadComplete(object sender, DevExpress.Web.ASPxUploadControl.FilesUploadCompleteEventArgs e)
        {
            ASPxUploadControl upload = sender as ASPxUploadControl;

            if (upload.UploadedFiles.Any(o => o.ContentLength > 0))
            {
                int key = FindKeyValueForUploader(upload);

                MusicArchiveContext ctx = EntitiesFactory.AsSingleton();
                SheetMusic sheetMusic = ctx.SheetMusic.FirstOrDefault(o => o.Id == key);
                parts = ctx.MusicParts.ToList(); //in-memory cache

                ZipFile zip = null;
                string errors = string.Empty;

                try
                {
                    string zipFileName = !string.IsNullOrEmpty(sheetMusic.ArchiveFileName) ? sheetMusic.ArchiveFileName : Master.PDF_CONTENT_DIR + "\\" + sheetMusic.Id + "_" + sheetMusic.Title + ".zip";
                    zip = File.Exists(zipFileName) ? ZipFile.Read(zipFileName) : new ZipFile(zipFileName);
                    zip.TempFileFolder = Master.TempUploadDirectory;

                    foreach (UploadedFile file in upload.UploadedFiles.Where(o => o.ContentLength > 0))
                    {
                        MusicPart part = FindPartForFileName(Path.GetFileNameWithoutExtension(file.FileName));

                        if (part == null || zip.Entries.Any(o => o.FileName == part.PartName + ".pdf"))
                        {
                            errors += "- " + file.FileName + "\r\n";
                            continue;
                        }

                        ctx.SheetMusicParts.AddObject(new SheetMusicPart() { SheetMusicId = sheetMusic.Id, MusicPartId = part.Id });
                        zip.AddEntry(part.PartName + ".pdf", file.FileContent);
                    }

                    zip.Save();
                    sheetMusic.ArchiveFileName = zip.Name;
                }
                finally
                {
                    if (zip != null)
                    {
                        zip.Dispose();
                    }
                }

                ctx.SaveChanges();

                if (!string.IsNullOrEmpty(errors))
                {
                    e.ErrorText = "Feil under innlasting av:\r\n" + errors + "\r\nFilene har galt navn eller eksisterer allerede";
                }
            }
        }

        #region Parts grid event handlers

        protected void PartsGrid_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            int partId = (int)(sender as ASPxGridView).GetRowValues(e.VisibleIndex, "Id");

            MusicArchiveContext ctx = EntitiesFactory.AsSingleton();
            User user = ctx.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            MailAddress from = new MailAddress(user.Email, user.Name);


            if (e.ButtonID == "btnSend")
            {
                SheetMusicPart part = ctx.SheetMusicParts.Include("MusicPart.Users").FirstOrDefault(o => o.Id == partId);

                if (part.MusicPart.Users.Where(o => !string.IsNullOrEmpty(o.Email) && !o.Inactive).Count() == 0) //no users defined for part
                {
                    throw new Exception("Kan ikke sende. Ingen musikant tilknyttet stemmen.");
                }

                SendEmailForPart(part, from);
            }
            else //btnSendToAddress
            {
                SheetMusicPart part = ctx.SheetMusicParts.FirstOrDefault(o => o.Id == partId);
                SendEmailForPart(part, EmailAddressInput.Value, from);
            }
        }

        protected void PartsGrid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName != "Users_Unbound")
            {
                return;
            }

            ASPxGridView partsGrid = sender as ASPxGridView;

            MusicPart currentRow = e.GetFieldValue("MusicPart") as MusicPart;
            string users = string.Empty;

            foreach (User user in currentRow.Users.Where(o => !o.Inactive))
            {
                users += string.Format("{0} ({1}), ", user.Name, user.Email);
            }

            if (users.Contains(","))
            {
                users = users.Remove(users.LastIndexOf(","));
            }

            e.DisplayText = users;
        }

        protected void PartsGrid_BeforePerformDataSelect(object sender, EventArgs e)
        {
            Session["SheetMusicId"] = (sender as ASPxGridView).GetMasterRowKeyValue();
        }

        protected void PartsGrid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            MusicArchiveContext context = EntitiesFactory.AsSingleton();

            int key = (int)e.Keys[0];
            SheetMusicPart sheetMusicPart = context.SheetMusicParts.Include("SheetMusic").FirstOrDefault(o => o.Id == key);

            ZipFile zipForSheet = ZipFile.Read(sheetMusicPart.SheetMusic.ArchiveFileName);
            zipForSheet.RemoveEntry(sheetMusicPart.MusicPart.PartName + ".pdf");
            zipForSheet.Save();

            context.DeleteObject(sheetMusicPart);
            context.SaveChanges();

            e.Cancel = true;
        }

        protected void PartsGrid_Init(object sender, EventArgs e)
        {
            ASPxGridView grid = (sender as ASPxGridView);
            grid.ForceDataRowType(typeof(SheetMusicPart));
            grid.Styles.CommandColumnItem.Paddings.PaddingLeft = Unit.Pixel(5);

            GridViewDataComboBoxColumn partColum = grid.Columns["MusicPartId"] as GridViewDataComboBoxColumn;
            partColum.PropertiesComboBox.DataSource = LookupRetriever.GetPartLookup();

            if (Master.GetGroupForUser() != UserGroupType.Administrator)
            {
                grid.Columns["SendPart"].Visible = false;
                grid.Columns["SendPartToAddress"].Visible = false;
                HideCommandButtons(grid);
            }
        }

        protected void PartsGrid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            (sender as ASPxGridView).CancelEdit();
            e.Cancel = true;
        }

        #endregion Parts grid event handlers

        /// <summary>
        /// Sends all the parts of the sheet with id <paramref name="sheetMusicId"/> to their corresponding part users
        /// </summary>
        /// <param name="sheetMusicId">The ID of the sheet to sende.</param>
        private void SendEmailsForMusicSheet(int sheetMusicId)
        {
            using (MusicArchiveContext ctx = new MusicArchiveContext())
            {
                SheetMusic sett = ctx
                    .SheetMusic
                    .Include("SheetMusicParts")
                    .FirstOrDefault(o => o.Id == sheetMusicId);

                User user = ctx.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                MailAddress sender = new MailAddress(user.Email, user.Name);

                foreach (SheetMusicPart part in sett.SheetMusicParts)
                {
                    SendEmailForPart(part, sender);
                }
            }
        }

        private void SendEmailForPart(SheetMusicPart sheetMusicPart, MailAddress sender)
        {
            if (sheetMusicPart.MusicPart.Users.Where(o => !string.IsNullOrEmpty(o.Email) && !o.Inactive).Count() == 0) //no users defined for part
            {
                return;
            }

            string recipients = string.Empty;

            foreach (User user in sheetMusicPart.MusicPart.Users.Where(o => !string.IsNullOrEmpty(o.Email) && !o.Inactive))
            {
                recipients += (user.Email + ",");
            }

            recipients = recipients.Remove(recipients.LastIndexOf(",")); //remove the last separator

            SendEmailForPart(sheetMusicPart, recipients, sender);
        }

        /// <summary>
        /// Sends the part with belonging .pdf as email with attachment
        /// </summary>
        /// <param name="sheetMusicPart">The part to send</param>
        private void SendEmailForPart(SheetMusicPart sheetMusicPart, string recipients, MailAddress sender)
        {
            if (string.IsNullOrEmpty(recipients))
            {
                return;
            }

            SmtpClient smtp = new SmtpClient();
            MailMessage email = new MailMessage();

            email.From = sender;
            email.To.Add(recipients);
            email.Subject = string.Format("{0} - {1} ", sheetMusicPart.SheetMusic.Title, sheetMusicPart.MusicPart.PartName);
            email.Body = string.Format("Vedlagt er PDF for {0} - {1}", sheetMusicPart.SheetMusic.Title, sheetMusicPart.MusicPart.PartName);

            using (ZipFile zip = ZipFile.Read(sheetMusicPart.SheetMusic.ArchiveFileName))
            {
                ZipEntry entry = zip.Entries.FirstOrDefault(o => o.FileName == sheetMusicPart.MusicPart.PartName + ".pdf");
                string tmpFile = Master.TempUploadDirectory + "\\" + Path.GetFileName(Path.GetTempFileName());

                using (FileStream fs = new FileStream(tmpFile, FileMode.Create, FileAccess.ReadWrite))
                {
                    entry.Extract(fs);
                }

                using (FileStream fs = new FileStream(tmpFile, FileMode.Open, FileAccess.Read))
                {
                    email.Attachments.Add(new Attachment(fs, string.Format("{0} - {1}", sheetMusicPart.SheetMusic.Title, sheetMusicPart.MusicPart.PartName), "application/pdf"));
                    smtp.Send(email);
                }
            }
        }

        /// <summary>
        /// Interpretes <paramref name="fileName"/> to locate the correct part
        /// </summary>
        /// <param name="fileName">The name of the file that was uploaded</param>
        /// <returns>The correct part (or null if not found)</returns>
        private MusicPart FindPartForFileName(string fileName)
        {
            MusicArchiveContext ctx = EntitiesFactory.AsSingleton();
            string strippedPartName = StripString(fileName);

            return parts.Where(part =>
                {
                    //first, check if part name matches
                    if (StripString(part.PartName) == strippedPartName)
                    {
                        return true;
                    }
                    else if (!string.IsNullOrEmpty(part.Aliases)) //no match for part name, check aliases
                    {
                        foreach (string alias in part.Aliases.Split(','))
                        {
                            if (StripString(alias) == strippedPartName)
                            {
                                return true;
                            }
                        }
                    }

                    //not a match
                    return false;
                }).FirstOrDefault();
        }

        private string StripString(string theString)
        {
            return theString.ToLower().Replace(" ", "").Replace(".", "");
        }

        /// <summary>
        /// Finds the expanded rows key value (music sheet ID)
        /// </summary>
        /// <param name="upload">The control that sent triggered event</param>
        /// <returns>The master row key value</returns>
        private int FindKeyValueForUploader(ASPxUploadControl upload)
        {
            GridViewBaseRowTemplateContainer container = null;
            Control control = upload;

            while (control.Parent != null)
            {
                container = control.Parent as GridViewBaseRowTemplateContainer;

                if (container != null)
                {
                    break;
                }

                control = control.Parent;
            }

            return (int)container.KeyValue;
        }

        private void HideCommandButtons(ASPxGridView grid)
        {
            GridViewCommandColumn commandCol = grid.Columns[0] as GridViewCommandColumn;
            commandCol.NewButton.Visible = false;
            commandCol.EditButton.Visible = false;
            commandCol.DeleteButton.Visible = false;
        }

        protected void SheetMusicGrid_RowValidating(object sender, ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["Title"].ToString().Contains("\""))
            {
                e.Errors[SheetMusicGrid.Columns["Title"]] = "Kan dessverre ikke ha anførselstegn i tittle";
                e.RowError = "Fiks feil før du lagrer";
            }
        }
    }
}