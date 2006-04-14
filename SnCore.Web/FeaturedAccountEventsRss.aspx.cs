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
using SnCore.WebServices;
using SnCore.BackEndServices;
using SnCore.Services;
using System.Collections.Generic;

public partial class FeaturedAccountEventsRss : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ServiceQueryOptions queryoptions = new ServiceQueryOptions();
                queryoptions.PageNumber = 0;
                queryoptions.PageSize = 25;

                rssRepeater.DataSource = SystemService.GetFeatures("AccountEvent", queryoptions);
                rssRepeater.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public string WebsiteUrl
    {
        get
        {
            return SessionManager.WebsiteUrl;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        Response.ContentType = "text/xml";
        base.OnPreRender(e);
    }

    public string Link
    {
        get
        {
            return WebsiteUrl.TrimEnd('/') + "/FeaturedAccountEventsView.aspx";
        }
    }

    public TransitAccountEvent GetAccountEvent(int id)
    {
        TransitAccountEvent a = (TransitAccountEvent)Cache[string.Format("accountevent:{0}", id)];
        if (a == null)
        {
            a = EventService.GetAccountEventById(SessionManager.Ticket, id);
            Cache.Insert(string.Format("accountevent:{0}", id),
                a, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
        }

        return a;
    }
}