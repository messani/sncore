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
using SnCore.WebServices;
using SnCore.Services;

public partial class AccountAttributesManage : AuthenticatedPage
{
    private TransitAccount mAccount = null;
    public TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                mAccount = AccountService.GetAccountById(RequestId);
            }

            return mAccount;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            linkNew.NavigateUrl = string.Format("AccountAttributeEdit.aspx?aid={0}", RequestId);
            accountLink.HRef = string.Format("AccountView.aspx?id={0}", RequestId);
            accountImage.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", Account.PictureId);
            accountName.Text = Render(Account.Name);

            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

            if (!IsPostBack)
            {
                GetData(sender, e);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = AccountService.GetAccountAttributesCountById(RequestId);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    private enum Cells
    {
        id = 0
    };

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageSize = gridManage.PageSize;
            options.PageNumber = gridManage.CurrentPageIndex;
            gridManage.DataSource = AccountService.GetAccountAttributesById(
                RequestId, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Delete":
                    int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                    AccountService.DeleteAccountAttribute(SessionManager.Ticket, id);
                    ReportInfo("Account attribute deleted.");
                    gridManage.CurrentPageIndex = 0;
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}