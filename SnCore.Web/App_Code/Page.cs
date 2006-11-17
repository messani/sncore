using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SnCore.Tools.Web;
using System.Reflection;
using System.Text.RegularExpressions;
using SnCore.WebServices;
using SnCore.BackEndServices;
using Microsoft.Web.UI;
using System.Text;
using System.Collections.Generic;
using System.Threading;

public class Page : System.Web.UI.Page
{
    private HtmlMeta mMetaDescription = null;
    protected SessionManager mSessionManager = null;

    protected override void OnPreInit(EventArgs e)
    {
        if (Master != null && Master is MasterPage)
        {
            ((MasterPage)Master).OnPagePreInit(e);
        }

        base.OnPreInit(e);
    }

    protected override void OnLoad(EventArgs e)
    {
        if (Header != null)
        {
            Header.Controls.Add(MetaDescription);
        }

        base.OnLoad(e);
    }

    public int RequestId
    {
        get
        {
            return GetId("id");
        }
    }

    public int GetId(string querystring)
    {
        string id = Request.QueryString[querystring];
        if (string.IsNullOrEmpty(id)) return 0;
        return int.Parse(id);
    }

    public string AdjustToRFC822(object dt)
    {
        return AdjustToRFC822((DateTime)dt);
    }

    public string AdjustToRFC822(DateTime dt)
    {
        return SessionManager.AdjustToRFC822(dt);
    }

    public DateTime ToUTC(DateTime dt)
    {
        return SessionManager.ToUTC(dt);
    }

    public DateTime Adjust(DateTime dt)
    {
        return SessionManager.Adjust(dt);
    }

    public DateTime Adjust(object dt)
    {
        return Adjust((DateTime)dt);
    }

    public string GetValue(object s, string defaultvalue)
    {
        return SessionManager.GetValue(s, defaultvalue);
    }

    public string GetValue(string s, string defaultvalue)
    {
        return SessionManager.GetValue(s, defaultvalue);
    }

    protected WebContentService ContentService
    {
        get
        {
            return SessionManager.ContentService;
        }
    }

    protected WebAccountService AccountService
    {
        get
        {
            return SessionManager.AccountService;
        }
    }

    protected WebSocialService SocialService
    {
        get
        {
            return SessionManager.SocialService;
        }
    }

    protected WebLocationService LocationService
    {
        get
        {
            return SessionManager.LocationService;
        }
    }

    protected WebSystemService SystemService
    {
        get
        {
            return SessionManager.SystemService;
        }
    }

    protected WebMarketingService MarketingService
    {
        get
        {
            return SessionManager.MarketingService;
        }
    }

    protected WebMadLibService MadLibService
    {
        get
        {
            return SessionManager.MadLibService;
        }
    }

    protected WebEventService EventService
    {
        get
        {
            return SessionManager.EventService;
        }
    }

    protected WebDiscussionService DiscussionService
    {
        get
        {
            return SessionManager.DiscussionService;
        }
    }

    protected WebStoryService StoryService
    {
        get
        {
            return SessionManager.StoryService;
        }
    }

    protected WebBugService BugService
    {
        get
        {
            return SessionManager.BugService;
        }
    }

    public WebTagWordService TagWordService
    {
        get
        {
            return SessionManager.TagWordService;
        }
    }

    public WebSyndicationService SyndicationService
    {
        get
        {
            return SessionManager.SyndicationService;
        }
    }

    public WebBackEndService BackEndService
    {
        get
        {
            return SessionManager.BackEndService;
        }
    }

    public WebPlaceService PlaceService
    {
        get
        {
            return SessionManager.PlaceService;
        }
    }

    public WebBlogService BlogService
    {
        get
        {
            return SessionManager.BlogService;
        }
    }

    public WebStatsService StatsService
    {
        get
        {
            return SessionManager.StatsService;
        }
    }

    public virtual SessionManager SessionManager
    {
        get
        {
            if (mSessionManager == null)
            {
                mSessionManager = new SessionManager(this);
            }
            return mSessionManager;
        }
    }

    public Page()
    {

    }

    public void Redirect(string url)
    {
        Response.Redirect(url, true);
    }

    public string Render(object o)
    {
        return Renderer.Render(o);
    }

    public virtual string RenderEx(object o)
    {
        return SessionManager.RenderMarkups(Renderer.RenderEx(o));
    }

    public virtual string RenderEx(string s)
    {
        return SessionManager.RenderMarkups(Renderer.RenderEx(s));
    }

    public string UrlEncode(object o)
    {
        return Renderer.UrlEncode(o);
    }

    public string UrlDecode(object o)
    {
        return Renderer.UrlDecode(o);
    }

    private static void RethrowException(Exception ex)
    {
        if (ex is ThreadAbortException)
            throw ex;
    }

    public void ReportException(Exception ex)
    {
        RethrowException(ex);
        if (Master == null) throw ex;
        object notice = Master.FindControl("noticeMenu");
        if (notice == null) throw ex;
        notice.GetType().GetProperty("Exception").SetValue(notice, ex, null);
    }

    public void ReportInfo(string message, bool htmlencode)
    {
        if (Master == null) throw new Exception(message);
        object notice = Master.FindControl("noticeMenu");
        if (notice == null) throw new Exception(message);
        notice.GetType().GetProperty("Info").SetValue(notice, message, null);
        notice.GetType().GetProperty("HtmlEncode").SetValue(notice, htmlencode, null);
    }

    public HtmlMeta MetaDescription
    {
        get
        {
            if (mMetaDescription == null)
            {
                mMetaDescription = new HtmlMeta();
                mMetaDescription.Name = "description";
                mMetaDescription.Content = SessionManager.GetCachedConfiguration(
                    "SnCore.Description", string.Empty);
            }
            return mMetaDescription;
        }
    }

    public void ReportInfo(string message)
    {
        ReportInfo(message, false);
    }

    public void ReportWarning(string message)
    {
        if (Master == null) throw new Exception(message);
        object notice = Master.FindControl("noticeMenu");
        if (notice == null) throw new Exception(message);
        notice.GetType().GetProperty("Warning").SetValue(notice, message, null);
    }

    public void SetDefaultButton(Button button)
    {
        PageManager.SetDefaultButton(button, this.Controls);
    }

    public string GetSummary(string summary)
    {
        return Renderer.GetSummary(SessionManager.RenderMarkups(summary));
    }

    public void RedirectToLogin()
    {
        Redirect(string.Format("AccountLogin.aspx?ReturnUrl={0}&AuthenticatedPage=true",
            Renderer.UrlEncode(Request.Url.PathAndQuery)));
    }
}

public class AuthenticatedPage : Page
{
    public AuthenticatedPage()
    {

    }

    protected override void OnLoad(EventArgs e)
    {
        if (string.IsNullOrEmpty(SessionManager.Ticket))
        {
            RedirectToLogin();
        }

        base.OnLoad(e);
    }
}

