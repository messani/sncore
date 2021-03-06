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
using SnCore.Services;
using SnCore.Tools.Web;

public partial class EmailAccountFriendRequestReject : AuthenticatedPage
{
    private TransitAccountFriendRequest mAccountFriendRequest;

    public TransitAccountFriendRequest AccountFriendRequest
    {
        get
        {
                if (mAccountFriendRequest == null)
                {
                    mAccountFriendRequest = SessionManager.SocialService.GetAccountFriendRequestById(
                        SessionManager.Ticket, RequestId);
                }
            return mAccountFriendRequest;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
            Title = string.Format("{0} declined your request", Renderer.Render(AccountFriendRequest.KeenName));
            panelMessage.Visible = ! string.IsNullOrEmpty(Request.QueryString["message"]);
    }
}

