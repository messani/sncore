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
using SnCore.Tools.Web;
using SnCore.Services;
using SnCore.WebServices;

public partial class DiscussionTopOfThreadsRss : Page
{
    public string WebsiteUrl
    {
        get
        {
            return SessionManager.WebsiteUrl;
        }
    }

    public string RssTitle
    {
        get
        {
            return Renderer.Render(string.Format("{0} Discussion Posts",
                SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore")));
        }
    }

    public string Link
    {
        get
        {
            return WebsiteUrl + "/DiscussionTopOfThreadsView.aspx";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ServiceQueryOptions options = new ServiceQueryOptions();
                options.PageSize = 50;
                options.PageNumber = 0;
                object[] args = { SessionManager.Ticket, options };
                rssRepeater.DataSource = SessionManager.GetCachedCollection<TransitDiscussionPost>(
                    SessionManager.DiscussionService, "GetDiscussionTopOfThreads", args);
                rssRepeater.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        Response.ContentType = "text/xml";
        base.OnPreRender(e);
    }
}
