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
using System.Text;

public partial class AccountMessageEdit : AuthenticatedPage
{
    public int ParentId
    {
        get
        {
            return GetId("pid");
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {
                TransitAccount ta = AccountService.GetAccountById(RequestId);
                imageAccountTo.ImageUrl = "AccountPictureThumbnail.aspx?id=" + ta.PictureId.ToString();
                linkAccountTo.Text = Renderer.Render(ta.Name);
                linkAccountTo.NavigateUrl = linkAccountTo2.HRef = "AccountView.aspx?id=" + ta.Id.ToString();
                linkBack.NavigateUrl = Renderer.UrlDecode(Request.QueryString["ReturnUrl"]);

                StringBuilder body = new StringBuilder();

                if (ParentId != 0)
                {
                    TransitAccountMessage rp = AccountService.GetAccountMessageById(
                        SessionManager.Ticket, ParentId);
                    panelReplyTo.Visible = true;
                    
                    messageFrom.NavigateUrl = accountlink.HRef = "AccountView.aspx?id=" + rp.SenderAccountId.ToString();

                    messageFrom.Visible = labelMessageFrom.Visible = (rp.SenderAccountId != SessionManager.Account.Id);
                    messageTo.Visible = labelMessageTo.Visible = (rp.RecepientAccountId != SessionManager.Account.Id);

                    replytoAccount.Text = messageFrom.Text = Renderer.Render(rp.SenderAccountName);
                    messageBody.Text = RenderEx(rp.Body);
                    messageSent.Text = rp.Sent.ToString();
                    replytoImage.ImageUrl = "AccountPictureThumbnail.aspx?id=" + rp.SenderAccountPictureId.ToString();
                    messageSubject.Text = Renderer.Render(rp.Subject);
                    inputSubject.Text = rp.Subject.StartsWith("Re:") ? rp.Subject : "Re: " + rp.Subject;
                    
                    body.AppendFormat("<P>[quote]<DIV>on {0} {1} wrote:</DIV><DIV>{2}</DIV>[/quote]</P>",
                            rp.Sent.ToString("d"), rp.SenderAccountName, rp.Body);
                }

                if (! string.IsNullOrEmpty(SessionManager.Account.Signature))
                {
                    body.Append("<BR /><BR />");
                    body.Append("<P>");
                    body.Append(Renderer.RenderEx(SessionManager.Account.Signature));
                    body.Append("</P>");
                }

                inputBody.Text = body.ToString();

                if (!AccountService.HasVerifiedEmail(SessionManager.Ticket))
                {
                    ReportWarning("You don't have any verified e-mail addresses.\n" +
                        "You must add/confirm a valid e-mail address before posting messages.");

                    manageAdd.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        try
        {
            TransitAccountMessage tw = new TransitAccountMessage();

            tw.Subject = inputSubject.Text;
            if (string.IsNullOrEmpty(tw.Subject)) tw.Subject = "Untitled";
            tw.Body = inputBody.Text;
            tw.AccountId = RequestId;
            tw.AccountMessageFolderId = 0;

            AccountService.AddAccountMessage(SessionManager.Ticket, tw);
            Redirect(Request.QueryString["ReturnUrl"]);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}
