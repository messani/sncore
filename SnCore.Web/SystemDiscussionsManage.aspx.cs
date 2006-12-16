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
using SnCore.WebServices;
using SnCore.SiteMap;

public partial class SystemDiscussionsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
                GetData(sender, e);

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Discussions", Request.Url));
                StackSiteMap(sitemapdata);
            }
    }

    private void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.DiscussionService.GetAccountDiscussionsCount(SessionManager.Ticket);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(gridManage.PageSize, gridManage.CurrentPageIndex);
        gridManage.DataSource = SessionManager.DiscussionService.GetAccountDiscussions(SessionManager.Ticket, options);
    }


    private enum Cells
    {
        id = 0
    };

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
            switch (e.CommandName)
            {
                case "Delete":
                    int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                    SessionManager.DiscussionService.DeleteDiscussion(SessionManager.Ticket, id);
                    ReportInfo("Discussion deleted.");
                    GetData(sender, e);
                    break;
            }
    }
}
