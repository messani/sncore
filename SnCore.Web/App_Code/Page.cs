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
using System.Text;
using System.Collections.Generic;
using System.Threading;
using SnCore.SiteMap;
using SnCore.Services;
using SnCore.WebControls;
using System.Globalization;

public class Page : System.Web.UI.Page
{
    protected SessionManager mSessionManager = null;
    protected bool mIsMobileEnabled = false;

    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
        }
        catch (Exception ex)
        {            
            ReportException(ex);
        }
    }

    protected override void InitializeCulture()
    {
        Culture = string.Format("auto:{0}", CultureInfo.InstalledUICulture.Name);
        UICulture = "auto";

        if (SessionManager.IsLoggedIn && SessionManager.Account.LCID > 0)
        {
            CultureInfo ci = new CultureInfo(SessionManager.Account.LCID);
            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
        }
        else
        {
            HttpCookie culture_cookie = Request.Cookies[SessionManager.sSnCoreCulture];
            if (culture_cookie != null)
            {
                try
                {
                    CultureInfo ci = new CultureInfo(int.Parse(culture_cookie.Value));
                    Thread.CurrentThread.CurrentUICulture = ci;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                }
                catch
                {
                    Response.Cookies.Add(new HttpCookie(SessionManager.sSnCoreCulture));
                }
            }
        }

        base.InitializeCulture();
    }

    protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
    {
        try
        {
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected override void OnPreInit(EventArgs e)
    {
        System.Web.UI.MasterPage master = Master;
        while (master != null)
        {
            if (master is MasterPage)
            {
                ((MasterPage)master).OnPagePreInit(e);
            }

            master = master.Master;
        }

        base.OnPreInit(e);
    }

    protected override void OnLoad(EventArgs e)
    {
        try
        {
            if (IsPostBack && Header != null)
            {
                Title = (string)ViewState["Title"];
            }

            SiteMapDataAttribute[] attributes = (SiteMapDataAttribute[])this.GetType().GetCustomAttributes(typeof(SiteMapDataAttribute), true);
            if (attributes != null && attributes.Length > 0)
            {
                SiteMapDataAttribute attribute = attributes[0];
                StackSiteMap(attribute, Request.Url.PathAndQuery);
            }

            base.OnLoad(e);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        try
        {
            base.OnPreRender(e);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!IsPostBack && Page.Header != null)
        {
            ViewState["Title"] = Title;
        }

        base.OnLoadComplete(e);
    }

    public SiteMapNode StackSiteMap(SiteMapDataAttribute attribute)
    {
        return StackSiteMap(attribute, Request.Url.PathAndQuery);
    }

    public SiteMapNode StackSiteMap(SiteMapDataAttribute attribute, string uri)
    {
        if (SiteMap.Provider is SiteMapDataProvider)
        {
            SiteMapDataProvider provider = (SiteMapDataProvider)SiteMap.Provider;
            return provider.Stack(attribute, uri);
        }
        else
        {
            throw new Exception("Site Map provider is not a data provider");
        }
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
        int result = 0;
        int.TryParse(id, out result);
        return result;
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

    public object FindNoticeMenuControl()
    {
        System.Web.UI.MasterPage master = Master;
        object notice = null;

        while (notice == null && master != null)
        {
            notice = master.FindControl("noticeMenu");
            master = master.Master;
        }

        return notice;
    }

    public void ReportException(Exception ex)
    {
        //
        // BUGBUG: this doesn't work with a remote SOAP back-end
        //         the exception becomes a Exception that needs to be parsed
        //

        if (ex is ManagedAccount.AccessDeniedException 
            && !SessionManager.IsLoggedIn)
        {
            RedirectToLogin();
            return;
        }

        if (ex is ManagedAccount.AccessDeniedException
            && !string.IsNullOrEmpty((ex as ManagedAccount.AccessDeniedException).RequestUri))
        {
            Redirect((ex as ManagedAccount.AccessDeniedException).RequestUri);
            return;
        }

        RethrowException(ex);
        if (Master == null) throw ex;
        object notice = FindNoticeMenuControl();
        if (notice == null) throw ex;
        notice.GetType().GetProperty("Exception").SetValue(notice, ex, null);
    }

    public void ReportInfo(string message, bool htmlencode)
    {
        if (Master == null) throw new Exception(message);
        object notice = FindNoticeMenuControl();
        if (notice == null) throw new Exception(message);
        notice.GetType().GetProperty("HtmlEncode").SetValue(notice, htmlencode, null);
        notice.GetType().GetProperty("Info").SetValue(notice, message, null);
    }

    public void ReportInfo(string message)
    {
        ReportInfo(message, false);
    }

    public void ReportWarning(string message)
    {
        if (Master == null) throw new Exception(message);
        object notice = FindNoticeMenuControl();
        if (notice == null) throw new Exception(message);
        notice.GetType().GetProperty("Warning").SetValue(notice, message, null);
    }

    public void SetDefaultButton(System.Web.UI.WebControls.Button button)
    {
        PageManager.SetDefaultButton(button, this.Controls);
    }

    public string GetSummary(string summary)
    {
        return Renderer.GetSummary(SessionManager.RenderMarkups(summary));
    }

    public void RedirectToLogin(string uri)
    {
        Redirect(string.Format("AccountLogin.aspx?ReturnUrl={0}&AuthenticatedPage=true",
            Renderer.UrlEncode(uri)));
    }

    public void RedirectToLogin()
    {
        RedirectToLogin(Request.Url.PathAndQuery);
    }

    public bool IsMobileEnabled
    {
        get
        {
            return mIsMobileEnabled;
        }
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

