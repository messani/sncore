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

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            accountsNewMain.DataBind();

            websiteBlog.BlogId = int.Parse(SessionManager.GetCachedConfiguration(
                    "SnCore.Blog.Id", "0"));

            websiteBlog.Visible = (websiteBlog.BlogId > 0);
        }
    }

    public string LinkMailToAdministrator
    {
        get
        {
            return string.Format("location.href='mailto:{0}';",
                SessionManager.GetCachedConfiguration(
                    "SnCore.Admin.EmailAddress", "admin@localhost.com"));
        }
    }
}