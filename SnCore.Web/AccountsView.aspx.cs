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

[SiteMapDataAttribute("People")]
public partial class AccountsView : AccountPersonPage
{
    public class SelectLocationEventArgs : EventArgs
    {
        public string Country;
        public string State;
        public string City;

        public SelectLocationEventArgs(TransitAccount account)
            : this(account.Country, account.State, account.City)
        {

        }

        public SelectLocationEventArgs(
            string country,
            string state,
            string city)
        {
            Country = country;
            State = state;
            City = city;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        SetDefaultButton(search);

        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            ArrayList countries = new ArrayList();
            countries.Add(new TransitCountry());
            object[] c_args = { null };
            countries.AddRange(SessionManager.GetCachedCollection<TransitCountry>(
                SessionManager.LocationService, "GetCountries", c_args));

            ArrayList states = new ArrayList();
            states.Add(new TransitState());

            inputCountry.DataSource = countries;
            inputCountry.DataBind();

            inputState.DataSource = states;
            inputState.DataBind();

            linkLocal.Visible = SessionManager.IsLoggedIn && !string.IsNullOrEmpty(SessionManager.Account.City);

            if (SessionManager.IsLoggedIn)
            {
                linkLocal.Text = string.Format("&#187; All {0} People", Renderer.Render(SessionManager.Account.City));
                SelectLocation(sender, new SelectLocationEventArgs(
                    Request["country"],
                    Request["state"],
                    Request["city"]));
            }

            GetData();
        }
    }

    public void gridManage_DataBinding(object sender, EventArgs e)
    {
        panelGrid.Update();
    }

    public void search_Click(object sender, EventArgs e)
    {
        GetData();
        panelGrid.Update();
    }

    public int AccountsCount
    {
        get
        {
            return SessionManager.GetCachedCollectionCount(
                SessionManager.SocialService, "GetAccountsCount", null);
        }
    }

    private void GetData()
    {
        gridManage.CurrentPageIndex = 0;
        object[] args = { GetQueryOptions() };
        gridManage.VirtualItemCount = SessionManager.GetCachedCollectionCount(
            SessionManager.SocialService, "GetAccountActivityCount", args);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
        labelCount.Text = string.Format("{0}/{1} people",
            gridManage.VirtualItemCount,
            AccountsCount);
    }

    public void inputCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArrayList states = new ArrayList();
        states.Add(new TransitState());
        object[] args = { inputCountry.SelectedValue };
        states.AddRange(SessionManager.GetCachedCollection<TransitState>(
            SessionManager.LocationService, "GetStatesByCountry", args));
        inputState.DataSource = states;
        inputState.DataBind();
        panelCountryState.Update();
    }

    private AccountActivityQueryOptions GetQueryOptions()
    {
        AccountActivityQueryOptions options = new AccountActivityQueryOptions();
        options.SortAscending = bool.Parse(listboxSelectOrderBy.SelectedValue);
        options.SortOrder = listboxSelectSortOrder.SelectedValue;
        options.PicturesOnly = checkboxPicturesOnly.Checked;
        options.BloggersOnly = checkboxBloggersOnly.Checked;
        options.City = inputCity.Text;
        options.Country = inputCountry.SelectedValue;
        options.State = inputState.SelectedValue;
        options.Name = inputName.Text;
        options.Email = inputEmailAddress.Text;
        return options;
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        AccountActivityQueryOptions options = GetQueryOptions();

        linkRelRss.NavigateUrl =
            string.Format("AccountsRss.aspx?order={0}&asc={1}&pictures={2}&city={3}&country={4}&state={5}&name={6}&email={7}&bloggers={8}",
                options.SortOrder,
                options.SortAscending,
                options.PicturesOnly,
                options.City,
                options.Country,
                options.State,
                Renderer.UrlEncode(options.Name),
                Renderer.UrlEncode(options.Email),
                options.BloggersOnly);

        ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
        serviceoptions.PageSize = gridManage.PageSize;
        serviceoptions.PageNumber = gridManage.CurrentPageIndex;
        object[] args = { options, serviceoptions };
        gridManage.DataSource = SessionManager.GetCachedCollection<TransitAccountActivity>(
            SessionManager.SocialService, "GetAccountActivity", args);
    }

    public void SelectLocation(object sender, SelectLocationEventArgs e)
    {
        try
        {
            inputCountry.ClearSelection();
            inputCountry.Items.FindByValue(e.Country).Selected = true;
            inputCountry_SelectedIndexChanged(sender, e);
            inputState.ClearSelection();
            inputState.Items.FindByValue(e.State).Selected = true;
            inputCity.Text = e.City;
        }
        catch
        {

        }
    }

    public void linkLocal_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsLoggedIn)
            return;

        checkboxBloggersOnly.Checked = false;
        checkboxPicturesOnly.Checked = false;
        inputName.Text = string.Empty;
        inputCity.Text = string.Empty;
        SelectLocation(sender, new SelectLocationEventArgs(SessionManager.Account));
        GetData();
        panelSearch.Update();
    }

    public void linkAll_Click(object sender, EventArgs e)
    {
        checkboxBloggersOnly.Checked = false;
        checkboxPicturesOnly.Checked = false;
        inputCountry.ClearSelection();
        inputState.ClearSelection();
        inputCity.Text = string.Empty;
        inputName.Text = string.Empty;
        GetData();
        panelSearch.Update();
    }

    public void linkBloggers_Click(object sender, EventArgs e)
    {
        checkboxPicturesOnly.Checked = false;
        checkboxBloggersOnly.Checked = true;
        inputCountry.ClearSelection();
        inputState.ClearSelection();
        inputCity.Text = string.Empty;
        inputName.Text = string.Empty;
        GetData();
        panelSearch.Update();
    }

    public void linkSearch_Click(object sender, EventArgs e)
    {
        panelSearchInternal.PersistentVisible = !panelSearchInternal.PersistentVisible;
        panelSearch.Update();
    }
}
