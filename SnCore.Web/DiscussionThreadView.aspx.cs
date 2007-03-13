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
using SnCore.SiteMap;

public partial class DiscussionThreadView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (RequestId == 0)
            {
                Redirect("Default.aspx");
                return;
            }

            TransitDiscussionThread t = SessionManager.DiscussionService.GetDiscussionThreadById(
                SessionManager.Ticket, RequestId);

            TransitDiscussion td = SessionManager.DiscussionService.GetDiscussionById(
                SessionManager.Ticket, t.DiscussionId);

            if (td.Personal)
            {              
                string uri = SessionManager.DiscussionService.GetThreadRedirectUri(
                    SessionManager.Ticket, td.Id);

                if (!string.IsNullOrEmpty(uri))
                {
                    Redirect(uri);
                    return;
                }
            }

            this.Title = Renderer.Render(td.Name);

            discussionMain.DiscussionThreadId = RequestId;
            discussionMain.DiscussionId = t.DiscussionId;
            discussionMain.DataBind();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();

            if (!string.IsNullOrEmpty(td.ParentObjectName))
            {
                sitemapdata.Add(new SiteMapDataAttributeNode(td.ParentObjectName, Request, td.ParentObjectUri));
                sitemapdata.Add(new SiteMapDataAttributeNode(td.Name, Request, string.Format("{0}#discuss", td.ParentObjectUri)));
                sitemapdata.Add(new SiteMapDataAttributeNode("Thread", Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("Discussions", Request, "DiscussionsView.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode(td.Name, Request, string.Format("DiscussionView.aspx?id={0}", td.Id)));
                sitemapdata.Add(new SiteMapDataAttributeNode("Thread", Request.Url));
            }
            StackSiteMap(sitemapdata);
        }
    }
}
