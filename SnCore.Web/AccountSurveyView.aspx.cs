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

public partial class AccountSurveyView : Page
{
    private TransitAccount mAccount = null;
    private TransitSurvey mSurvey = null;

    public void Page_Load(object sender, EventArgs e)
    {
        accountSurveyAnswers.OnGetDataSource += new EventHandler(accountSurveyAnswers_OnGetDataSource);
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            TransitSurvey s = Survey;

            if (AccountId > 0)
            {
                TransitAccount a = Account;
                accountName.Text = Renderer.Render(a.Name);
                accountImage.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", a.PictureId);
                accountLink.HRef = string.Format("AccountView.aspx?id={0}", a.Id);
                this.Title = string.Format("{0}'s {1}", Renderer.Render(a.Name), Renderer.Render(s.Name));

                sitemapdata.Add(new SiteMapDataAttributeNode("People", Request, "AccountsView.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode(a.Name, Request, string.Format("AccountView.aspx?id={0}", a.Id)));
            }
            else
            {
                accountcolumn.Visible = false;
                this.Title = string.Format("{0}", Renderer.Render(s.Name));
            }

            sitemapdata.Add(new SiteMapDataAttributeNode(s.Name, Request.Url));
            StackSiteMap(sitemapdata);

            surveyName.Text = Renderer.Render(s.Name);
            accountSurveyAnswers_OnGetDataSource(sender, e);
            accountSurveyAnswers.DataBind();
        }
    }

    public TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                mAccount = (SessionManager.Account != null && AccountId == SessionManager.Account.Id)
                    ? SessionManager.Account
                    : SessionManager.GetInstance<TransitAccount, int>(AccountId, SessionManager.AccountService.GetAccountById);
            }
            return mAccount;
        }
    }

    public TransitSurvey Survey
    {
        get
        {
            if (mSurvey == null)
            {
                mSurvey = SessionManager.ObjectService.GetSurveyById(
                    SessionManager.Ticket, SurveyId);
            }

            return mSurvey;
        }
    }

    void accountSurveyAnswers_OnGetDataSource(object sender, EventArgs e)
    {
        accountSurveyAnswers.DataSource = SessionManager.GetCollection<TransitAccountSurveyAnswer, int, int>(
            AccountId, SurveyId, (ServiceQueryOptions) null, SessionManager.AccountService.GetAccountSurveyAnswers);
    }

    public int AccountId
    {
        get
        {
            int result = GetId("aid");
            if (result > 0) return result;
            if (SessionManager.Account == null) return 0;
            return SessionManager.Account.Id;
        }
    }

    public int SurveyId
    {
        get
        {
            return RequestId;
        }
    }
}
