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
using System.Collections.Generic;

public partial class AccountMessageMove : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                linkCancel.NavigateUrl = ReturnUrl;

                TransitAccountMessage message = AccountService.GetAccountMessageById(
                    SessionManager.Ticket, RequestId);
                messageSenderLink.HRef = messageFrom.NavigateUrl = string.Format("AccountView.aspx?id={0}", message.SenderAccountId);
                messageFrom.Text = messageSenderName.Text = Renderer.Render(message.SenderAccountName);
                messageTo.NavigateUrl = string.Format("AccountView.aspx?id={0}", message.RecepientAccountId); 
                messageTo.Text = Renderer.Render(message.RecepientAccountName);
                messageSent.Text = message.Sent.ToString();
                messageSenderImage.ImageUrl = string.Format("AccountPictureThumbnail.aspx?id={0}", message.SenderAccountPictureId);
                messageSubject.Text = Renderer.Render(message.Subject);

                List<TransitAccountMessageFolder> folders = AccountService.GetAccountMessageFolders(SessionManager.Ticket);
                TransitAccountMessageFolder none = new TransitAccountMessageFolder();
                none.FullPath = none.Name = "Please choose ...";
                folders.Insert(0, none);
                listFolders.DataSource = folders;
                listFolders.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void listFolders_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int dest_id = int.Parse(listFolders.SelectedValue);
            AccountService.MoveAccountMessageToFolderById(SessionManager.Ticket, RequestId, dest_id);
            Redirect(ReturnUrl);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }        
    }

    public string ReturnUrl
    {
        get
        {
            object o = Request.QueryString["ReturnUrl"];
            return (o == null ? "AccountMessageFoldersManage.aspx?folder=inbox" : o.ToString());
        }
    }
}