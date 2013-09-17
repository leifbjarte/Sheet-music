using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ionic.Zip;
using System.IO.Compression;
using System.IO;

namespace SBBArkiv
{
    public partial class FileDownload : System.Web.UI.Page
    {
        private string TempUploadDirectory
        {
            get { return Server.MapPath("~/App_Data/UploadTemp"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request.QueryString["action"];

            if (action == "SendSheetAsZip")
            {
                int sheetId = int.Parse(Request.QueryString["sheetId"]);
                RetrieveZipForMusicSheet(sheetId);
            }
            else if (action == "DownloadSinglePdf")
            {
                string sheetMusicPartId = Request.QueryString["sheetPartId"];
                DownloadSinglePdf(int.Parse(sheetMusicPartId));
            }
        }

        private void DownloadSinglePdf(int sheetMusicPartId)
        {
            MusicArchiveContext ctx = EntitiesFactory.AsSingleton();

            SheetMusicPart sheetMusicPart = ctx.SheetMusicParts
              .Include("MusicPart")
              .Include("SheetMusic")
              .FirstOrDefault(o => o.Id == sheetMusicPartId);

            using (ZipFile zip = ZipFile.Read(sheetMusicPart.SheetMusic.ArchiveFileName))
            {
                ZipEntry file = zip.Entries.FirstOrDefault(o => o.FileName == sheetMusicPart.MusicPart.PartName + ".pdf");

                Response.ClearContent();
                Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", sheetMusicPart.SheetMusic.Title.Replace(" ", "_") + "_" + sheetMusicPart.MusicPart.PartName.Replace(" ", "_") + ".pdf"));
                Response.AddHeader("Content-Length", file.UncompressedSize.ToString());
                Response.Charset = "utf-8";
                Response.ContentType = "application/pdf";
                file.Extract(Response.OutputStream);
                Response.End();
            }
        }

        private void RetrieveZipForMusicSheet(int sheetMusicId)
        {
            SheetMusic sett;

            MusicArchiveContext ctx = EntitiesFactory.AsSingleton();

            sett = ctx
                .SheetMusic
                .Include("SheetMusicParts.MusicPart")
                .FirstOrDefault(o => o.Id == sheetMusicId);

            Response.ClearContent();
            Response.BufferOutput = false;
            Response.ContentType = "application/zip";
            Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", sett.Id + "_" + sett.Title.Replace(" ", "_") + ".zip"));
            byte[] file = File.ReadAllBytes(sett.ArchiveFileName);
            Response.AddHeader("Content-Length", file.Count().ToString());
            Response.BinaryWrite(file);
            Response.End();
        }
    }
}