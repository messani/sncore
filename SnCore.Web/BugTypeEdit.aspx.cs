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
using SnCore.SiteMap;
using SnCore.Data.Hibernate;

public partial class BugTypeEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Bugs", Request, "BugProjectsManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Types", Request, "BugTypesManage.aspx"));

            DomainClass cs = SessionManager.GetDomainClass("BugType");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;

            if (RequestId > 0)
            {
                TransitBugType t = SessionManager.BugService.GetBugTypeById(
                    SessionManager.Ticket, RequestId);
                inputName.Text = t.Name;
                sitemapdata.Add(new SiteMapDataAttributeNode(t.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Type", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitBugType t = new TransitBugType();
        t.Name = inputName.Text;
        t.Id = RequestId;
        SessionManager.CreateOrUpdate<TransitBugType>(
            t, SessionManager.BugService.CreateOrUpdateBugType);
        Redirect("BugTypesManage.aspx");
    }
}
