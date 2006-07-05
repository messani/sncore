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

public partial class AccountFeedItemImgsView : AccountPersonPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
                GetData();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private TransitAccountFeedItemImgQueryOptions mQueryOptions = null;

    TransitAccountFeedItemImgQueryOptions QueryOptions
    {
        get
        {
            if (mQueryOptions == null)
            {
                mQueryOptions = new TransitAccountFeedItemImgQueryOptions();
                mQueryOptions.InterestingOnly = false;
                mQueryOptions.VisibleOnly = (SessionManager.IsAdministrator ? false : true);
            }

            return mQueryOptions;
        }
    }

    private void GetData()
    {
        gridManage.CurrentPage = 0;
        gridManage.VirtualItemCount = SyndicationService.GetAccountFeedItemImgsCount(QueryOptions);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
        labelCount.Text = string.Format("{0} picture{1}",
            gridManage.VirtualItemCount, gridManage.VirtualItemCount == 1 ? string.Empty : "s");
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
            serviceoptions.PageSize = gridManage.PageSize;
            serviceoptions.PageNumber = gridManage.CurrentPage;
            gridManage.DataSource = SyndicationService.GetAccountFeedItemImgs(QueryOptions, serviceoptions);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void gridManage_ItemCommand(object sender, CommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Toggle":
                    TransitAccountFeedItemImg img = SyndicationService.GetAccountFeedItemImgById(
                        SessionManager.Ticket, int.Parse(e.CommandArgument.ToString()));
                    img.Visible = !img.Visible;
                    if (!img.Visible) img.Interesting = false;
                    SyndicationService.CreateOrUpdateAccountFeedItemImg(SessionManager.Ticket, img);
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