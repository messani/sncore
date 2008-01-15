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
using System.Web.Caching;
using SnCore.SiteMap;
using SnCore.Data.Hibernate;
using SnCore.WebControls;

public partial class AccountPreferencesManage : AuthenticatedPage
{
    private LocationSelectorCountryStateCityText mLocationSelector = null;

    public LocationSelectorCountryStateCityText LocationSelector
    {
        get
        {
            if (mLocationSelector == null)
            {
                mLocationSelector = new LocationSelectorCountryStateCityText(
                    this, true, inputCountry, inputState, inputCity);
            }

            return mLocationSelector;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        LocationSelector.LocationChanged += new EventHandler(LocationSelector_LocationChanged);

        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Preferences", Request.Url));
            StackSiteMap(sitemapdata);

            DomainClass cs = SessionManager.GetDomainClass("Account");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;
            inputCity.MaxLength = cs["City"].MaxLengthInChars;
            inputSignature.MaxLength = cs["Signature"].MaxLengthInChars;

            inputName.Text = SessionManager.Account.Name;

            inputBirthday.SelectedDate = SessionManager.Account.Birthday;
            inputBirthday.DataBind();
            inputCity.Text = SessionManager.Account.City;
            inputTimeZone.SelectedTzIndex = SessionManager.Account.TimeZone;

            LocationSelector.SelectLocation(sender, new LocationEventArgs(SessionManager.Account));
            LocationSelector_LocationChanged(sender, e);

            inputSignature.Text = SessionManager.Account.Signature;
            groups.AccountId = SessionManager.Account.Id;
            accountredirect.TargetUri = string.Format("AccountView.aspx?id={0}", SessionManager.Account.Id);
        }
    }

    void LocationSelector_LocationChanged(object sender, EventArgs e)
    {
        autoCompleteCity.ContextKey = string.Format("{0};{1}",
            inputCountry.Text, inputState.Text);
        panelCity.Update();
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccount ta = SessionManager.Account;
        ta.Birthday = inputBirthday.SelectedDate;
        ta.Name = inputName.Text;
        ta.City = inputCity.Text;
        ta.Country = inputCountry.SelectedValue;
        ta.State = inputState.SelectedValue;
        ta.TimeZone = inputTimeZone.SelectedTzIndex;
        ta.Signature = inputSignature.Text;

        if (ta.Signature.Length > inputSignature.MaxLength)
            throw new Exception(string.Format("Signature may not exceed {0} characters.", inputSignature.MaxLength));

        SessionManager.CreateOrUpdate<TransitAccount>(
            ta, SessionManager.AccountService.CreateOrUpdateAccount);
        Cache.Remove(string.Format("account:{0}", SessionManager.Ticket));
        ReportInfo("Profile saved.");
    }
}
