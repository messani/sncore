using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Wilco.Web.UI.WebControls;
using System.Drawing;
using SnCore.Tools.Drawing;
using SnCore.Tools.Web;
using System.IO;
using SnCore.Services;
using SnCore.WebServices;
using SnCore.Tools;
using SnCore.SiteMap;

public partial class AccountEventPicturesManage : AuthenticatedPage
{
    public void Page_Load()
    {
        this.addFile.Attributes["onclick"] = this.files.GetAddFileScriptReference() + "return false;";
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            TransitAccountEvent p = SessionManager.EventService.GetAccountEventById(SessionManager.Ticket, RequestId, SessionManager.UtcOffset);

            gridManage_OnGetDataSource(this, null);
            gridManage.DataBind();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Events", Request, "AccountEventsManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(p.Name, Request, string.Format("AccountEventView.aspx?id={0}", p.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request.Url));
            StackSiteMap(sitemapdata);
        }

        if (!SessionManager.AccountService.HasVerifiedEmail(SessionManager.Ticket))
        {
            ReportWarning("You don't have any verified e-mail addresses.\n" +
                "You must add/confirm a valid e-mail address before uploading pictures.");

            addFile.Enabled = false;
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        gridManage.DataSource = SessionManager.EventService.GetAccountEventPicturesById(RequestId, null);
    }

    private enum Cells
    {
        id = 0
    };

    public void gridManage_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
            case ListItemType.SelectedItem:
            case ListItemType.EditItem:
                int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                switch (e.CommandName)
                {
                    case "Delete":
                        SessionManager.EventService.DeleteAccountEventPicture(SessionManager.Ticket, id);
                        ReportInfo("Picture deleted.");
                        gridManage.CurrentPageIndex = 0;
                        gridManage_OnGetDataSource(source, e);
                        gridManage.DataBind();
                        break;
                }
                break;
        }
    }

    protected void files_FilesPosted(object sender, FilesPostedEventArgs e)
    {
        if (e.PostedFiles.Count == 0)
            return;

        ExceptionCollection exceptions = new ExceptionCollection();
        foreach (HttpPostedFile file in e.PostedFiles)
        {
            try
            {
                TransitAccountEventPictureWithPicture p = new TransitAccountEventPictureWithPicture();
                ThumbnailBitmap t = new ThumbnailBitmap(file.InputStream);
                p.Picture = t.Bitmap;
                p.Name = Path.GetFileName(file.FileName);
                p.Description = string.Empty;
                p.AccountEventId = RequestId;
                SessionManager.EventService.CreateOrUpdateAccountEventPicture(SessionManager.Ticket, p);
            }
            catch (Exception ex)
            {
                exceptions.Add(new Exception(string.Format("Error processing {0}: {1}",
                    Renderer.Render(file.FileName), ex.Message), ex));
            }
        }

        gridManage.CurrentPageIndex = 0;
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
        exceptions.Throw();
    }

}
