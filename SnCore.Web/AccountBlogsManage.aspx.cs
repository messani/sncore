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
using SnCore.SiteMap;
using SnCore.Services;

public partial class AccountBlogsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        gridManageAuthor.OnGetDataSource += new EventHandler(gridManageAuthor_OnGetDataSource);

        if (!IsPostBack)
        {
            gridManage_OnGetDataSource(this, null);
            gridManage.DataBind();

            gridManageAuthor_OnGetDataSource(this, null);
            gridManageAuthor.DataBind();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    private enum Cells
    {
        id = 0
    };

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        gridManage.DataSource = SessionManager.BlogService.GetAccountBlogs(
            SessionManager.Ticket, SessionManager.AccountId, null);
    }

    void gridManageAuthor_OnGetDataSource(object sender, EventArgs e)
    {
        gridManageAuthor.DataSource = SessionManager.BlogService.GetAuthoredAccountBlogs(
            SessionManager.Ticket, SessionManager.AccountId, null);
    }

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
        switch (e.CommandName)
        {
            case "Delete":
                SessionManager.Delete<TransitAccountBlog>(id, SessionManager.BlogService.DeleteAccountBlog);
                ReportInfo("Blog deleted.");
                gridManage.CurrentPageIndex = 0;
                gridManage_OnGetDataSource(sender, e);
                gridManage.DataBind();
                break;
        }
    }
}
