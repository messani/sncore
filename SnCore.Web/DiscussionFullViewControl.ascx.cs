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
using Wilco.Web.UI;
using Wilco.Web.UI.WebControls;
using SnCore.Services;

public partial class DiscussionFullViewControl : Control
{
    public string Text
    {
        get
        {
            return discussionLabel.Text;
        }
        set
        {
            discussionLabel.Text = value;
        }
    }

    public string PostNewText
    {
        get
        {
            return postNew.Text;
        }
        set
        {
            postNew.Text = value;
        }
    }


    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (DiscussionId > 0)
                {
                    GetData(sender, e);
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void GetData(object sender, EventArgs e)
    {
        TransitDiscussion d = SessionManager.DiscussionService.GetDiscussionById(DiscussionId);
        if (string.IsNullOrEmpty(discussionLabel.Text)) discussionLabel.Text = Renderer.Render(d.Name);
        discussionDescription.Text = Renderer.Render(d.Description);
        divDescription.Visible = ! string.IsNullOrEmpty(discussionDescription.Text);
        discussionView.DataSource = SessionManager.DiscussionService.GetDiscussionPosts(
            SessionManager.Ticket, DiscussionId, null);
        discussionView.DataBind();

        postNew.NavigateUrl = string.Format("DiscussionPost.aspx?did={0}&ReturnUrl={1}&#edit", 
            DiscussionId, Renderer.UrlEncode(ReturnUrl));
    }

    public void discussionView_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Delete":
                    {
                        int id = int.Parse(e.CommandArgument.ToString());
                        SessionManager.DiscussionService.DeleteDiscussionPost(SessionManager.Ticket, id);
                        discussionView.DataSource = SessionManager.DiscussionService.GetDiscussionPosts(
                            SessionManager.Ticket, DiscussionId, null);
                        discussionView.DataBind();
                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public int DiscussionId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "DiscussionId", 0);
        }
        set
        {
            ViewState["DiscussionId"] = value;

            if (IsPostBack)
            {
                GetData(this, null);
            }
        }
    }

    private enum Cells
    {
        id = 0,
        canedit,
        candelete,
        content,
        reply,
        edit,
        delete
    };

    public void discussionView_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
            case ListItemType.SelectedItem:
            case ListItemType.EditItem:
                int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                bool canedit = bool.Parse(e.Item.Cells[(int)Cells.canedit].Text);
                bool candelete = bool.Parse(e.Item.Cells[(int)Cells.candelete].Text);

                HtmlAnchor linkEdit = (HtmlAnchor) e.Item.FindControl("linkEdit");
                linkEdit.Visible = canedit;
                linkEdit.HRef = string.Format("DiscussionPost.aspx?did={0}&id={1}&ReturnUrl={2}&#edit",
                    DiscussionId, id, Renderer.UrlEncode(ReturnUrl));

                LinkButton linkDelete = (LinkButton) e.Item.FindControl("linkDelete");
                linkDelete.CommandArgument = id.ToString();
                linkDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this post?');");
                linkDelete.Visible = candelete;

                break;
        }
    }

    public string ReturnUrl
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<string>(ViewState, "ReturnUrl", Request.Url.PathAndQuery);
        }
        set
        {
            ViewState["ReturnUrl"] = value;
        }
    }

    public string GetCssClass(DateTime ts)
    {
        return (ts.AddDays(5) < DateTime.UtcNow) ? "sncore_message" : "sncore_new_message";
    }

    public string GetCssStyle(DateTime ts)
    {
        return (ts.AddDays(5) < DateTime.UtcNow) ? "display: none;" : string.Empty;
    }

    public string GetCssPictureStyle(DateTime ts, int len)
    {
        return (ts.AddDays(5) < DateTime.UtcNow || len < 64) ? "height: 50px;" : string.Empty;
    }
}
