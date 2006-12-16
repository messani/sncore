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
using System.Text;
using SnCore.SiteMap;

public partial class AccountPropertyGroupEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));

            if (RequestId > 0)
            {
                TransitAccountPropertyGroup tag = SessionManager.AccountService.GetAccountPropertyGroupById(RequestId);
                labelName.Text = Render(tag.Name);
                labelDescription.Text = Render(tag.Description);
                sitemapdata.Add(new SiteMapDataAttributeNode(tag.Name, Request.Url));
            }
            else
            {
                labelName.Text = "All Property Groups";
                sitemapdata.Add(new SiteMapDataAttributeNode("Properties", Request.Url));
            }

            StackSiteMap(sitemapdata);

            gridManage.DataSource = SessionManager.AccountService.GetAllAccountPropertyValues(SessionManager.Ticket, RequestId);
            gridManage.DataBind();
        }

        SetDefaultButton(save);
    }

    public void save_Click(object sender, EventArgs e)
    {
        foreach (DataGridItem item in gridManage.Items)
        {
            switch (item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                case ListItemType.SelectedItem:
                    int id = int.Parse(((HiddenField)item.FindControl("Id")).Value);
                    int property_id = int.Parse(((HiddenField)item.FindControl("propertyId")).Value);

                    TransitAccountProperty prop = SessionManager.AccountService.GetAccountPropertyById(property_id);

                    TransitAccountPropertyValue value = new TransitAccountPropertyValue();
                    value.Id = id;
                    value.AccountId = SessionManager.Account.Id;
                    value.AccountProperty = prop;
                    switch (prop.Type.ToString())
                    {
                        case "System.Array":
                            value.Value = StringToArray(((TextBox)item.FindControl("array_value")).Text);
                            break;
                        case "System.Text.StringBuilder":
                            value.Value = ((TextBox)item.FindControl("text_value")).Text;
                            break;
                        case "System.String":
                            value.Value = ((TextBox)item.FindControl("string_value")).Text;
                            break;
                        case "System.Int32":
                            value.Value = ((TextBox)item.FindControl("int_value")).Text;
                            break;
                        case "System.Boolean":
                            value.Value = ((CheckBox)item.FindControl("bool_value")).Checked.ToString();
                            break;
                    }

                    value.Id = SessionManager.AccountService.CreateOrUpdateAccountPropertyValue(
                        SessionManager.Ticket, value);
                    break;
            }
        }

        Redirect(linkBack.NavigateUrl);
    }

    private string StringToArray(string value)
    {
        string[] arr = value.Split(",;".ToCharArray());
        if (arr.Length == 0) return string.Empty;
        StringBuilder sb = new StringBuilder(value.Length);
        foreach (string s in arr)
        {
            sb.Append("\"" + s.Trim() + "\"");
        }
        return sb.ToString();
    }
}
