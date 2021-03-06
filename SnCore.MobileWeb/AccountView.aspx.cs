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
using SnCore.SiteMap;

public partial class AccountView : Page
{
    private int mAccountId = -1;
    private AccountService.TransitAccount mAccount = null;

    public int AccountId
    {
        get
        {
            if (mAccountId < 0)
            {
                mAccountId = RequestId;
                if (mAccountId == 0)
                {
                    mAccountId = SessionManager.Account.Id;
                }
            }

            return mAccountId;
        }
    }

    public AccountService.TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                mAccount = SessionManager.GetInstance<AccountService.TransitAccount, AccountService.ServiceQueryOptions, int>(
                    AccountId, SessionManager.AccountService.GetAccountById);
            }
            return mAccount;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Account == null)
            {
                throw new Exception("Account does not exist.");
            }

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("People", Request, "AccountsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(Account.Name, Request.Url));
            StackSiteMap(sitemapdata);

            this.Title = Renderer.Render(Account.Name);

            accountLastLogin.Text = string.Format("last activity: {0}",
                SessionManager.Adjust(Account.LastLogin).ToString("d"));

            accountCity.Text = Renderer.Render(Account.City);
            accountState.Text = Renderer.Render(Account.State);
            accountCountry.Text = Renderer.Render(Account.Country);

            accountImage.Visible = Account.PictureId != 0;
            accountImage.ImageUrl = string.Format("AccountPictureThumbnail.aspx?id={0}",
                Account.PictureId);

            accountLinkPictures.HRef = string.Format("AccountPicturesView.aspx?id={0}",
                Account.Id);

            GetTestimonials(sender, e);
            GetPictures(sender, e);
        }
    }

    private void GetPictures(object sender, EventArgs e)
    {
        linkPictures.NavigateUrl = string.Format("AccountPicturesView.aspx?id={0}", Account.Id);
    }

    private void GetTestimonials(object sender, EventArgs e)
    {
        int testimonials_id = SessionManager.GetCount<DiscussionService.TransitDiscussion, DiscussionService.ServiceQueryOptions, string, int>(
            "Account", Account.Id, SessionManager.DiscussionService.GetOrCreateDiscussionId);

        linkTestimonials.NavigateUrl = string.Format("DiscussionView.aspx?id={0}", testimonials_id);
    }
}
