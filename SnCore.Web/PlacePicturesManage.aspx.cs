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

public partial class PlacePicturesManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        this.addFile.Attributes["onclick"] = this.files.GetAddFileScriptReference() + "return false;";
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            TransitPlace place = SessionManager.PlaceService.GetPlaceById(SessionManager.Ticket, RequestId);

            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "PlacesView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(place.Name, Request, string.Format("PlaceView.aspx?id={0}", place.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request.Url));
            StackSiteMap(sitemapdata);
        }

        if (!SessionManager.HasVerified())
        {
            ReportWarning("You don't have any verified e-mail addresses and/or profile photos.\n" +
                "You must add/confirm a valid e-mail address and upload a profile photo before uploading place pictures.");

            addFile.Enabled = false;
            save.Enabled = false;
        }
    }

    public void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.PlaceService.GetPlacePicturesCount(
            SessionManager.Ticket, RequestId);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageSize = gridManage.PageSize;
        options.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.PlaceService.GetPlacePictures(
            SessionManager.Ticket, RequestId, options);
    }

    private enum Cells
    {
        id = 0
    };

    public void gridManage_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    SessionManager.Delete<TransitPlacePicture>(id, SessionManager.PlaceService.DeletePlacePicture);
                    ReportInfo("Picture deleted.");
                    GetData(sender, e);
                }
                break;
            case "Right":
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    SessionManager.PlaceService.MovePlacePicture(SessionManager.Ticket, id, 1);
                    if (e.Item.ItemIndex + 1 == gridManage.Items.Count && gridManage.CurrentPageIndex + 1 < gridManage.PagedDataSource.PageCount)
                        gridManage.CurrentPageIndex++;
                    SessionManager.InvalidateCache<TransitPlacePicture>();
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                }
                break;
            case "Left":
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    SessionManager.PlaceService.MovePlacePicture(SessionManager.Ticket, id, -1);
                    if (e.Item.ItemIndex == 0 && gridManage.CurrentPageIndex > 0) gridManage.CurrentPageIndex--;
                    SessionManager.InvalidateCache<TransitPlacePicture>();
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                }
                break;
        }
    }

    protected void files_FilesPosted(object sender, FilesPostedEventArgs e)
    {
        try
        {
            if (e.PostedFiles.Count == 0)
                return;

            ExceptionCollection exceptions = new ExceptionCollection();
            foreach (HttpPostedFile file in e.PostedFiles)
            {
                try
                {
                    TransitPlacePicture p = new TransitPlacePicture();
                    ThumbnailBitmap t = new ThumbnailBitmap(file.InputStream);
                    p.Bitmap = t.Bitmap;
                    p.Name = Path.GetFileName(file.FileName);
                    p.Description = string.Empty;
                    p.PlaceId = RequestId;
                    SessionManager.CreateOrUpdate<TransitPlacePicture>(
                        p, SessionManager.PlaceService.CreateOrUpdatePlacePicture);
                }
                catch (Exception ex)
                {
                    exceptions.Add(new Exception(string.Format("Error processing {0}: {1}",
                        Renderer.Render(file.FileName), ex.Message), ex));
                }
            }

            GetData(sender, e);
            exceptions.Throw();

            Redirect(string.Format("PlaceView.aspx?id={0}", RequestId));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

}
