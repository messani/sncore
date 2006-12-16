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

public partial class PlaceEdit : AuthenticatedPage
{
    private LocationSelectorCountryStateCityNeighborhoodText mLocationSelector = null;

    public LocationSelectorCountryStateCityNeighborhoodText LocationSelector
    {
        get
        {
            if (mLocationSelector == null)
            {
                mLocationSelector = new LocationSelectorCountryStateCityNeighborhoodText(
                    this, false, inputCountry, inputState, inputCity, inputNeighborhood);
            }

            return mLocationSelector;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridPlaceNamesManage.OnGetDataSource += new EventHandler(gridPlaceNamesManage_OnGetDataSource);

            LocationSelector.CountryChanged += new EventHandler(LocationSelector_CountryChanged);
            LocationSelector.StateChanged += new EventHandler(LocationSelector_StateChanged);

            if (!IsPostBack)
            {
                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "PlacesView.aspx"));

                gridPlaceNamesManage_OnGetDataSource(sender, e);
                gridPlaceNamesManage.DataBind();

                ppg.PlaceId = RequestId;
                ppg.DataBind();

                ArrayList types = new ArrayList();
                types.Add(new TransitAccountPlaceType());
                types.AddRange(SessionManager.PlaceService.GetPlaceTypes());
                selectType.DataSource = types;
                selectType.DataBind();

                if (RequestId > 0)
                {
                    TransitPlace place = SessionManager.PlaceService.GetPlaceById(SessionManager.Ticket, RequestId);
                    labelName.Text = Renderer.Render(place.Name);
                    inputName.Text = place.Name;
                    inputDescription.Text = place.Description;
                    inputCrossStreet.Text = place.CrossStreet;
                    inputEmail.Text = place.Email;
                    inputFax.Text = place.Fax;
                    inputPhone.Text = place.Phone;
                    inputStreet.Text = place.Street;
                    inputWebsite.Text = place.Website;
                    inputZip.Text = place.Zip;
                    selectType.Items.FindByValue(place.Type).Selected = true;
                    LocationSelector.SelectLocation(sender, new LocationEventArgs(place));
                    linkEditAttributes.NavigateUrl = string.Format("PlaceAttributesManage.aspx?id={0}", place.Id);
                    linkEditPictures.NavigateUrl = string.Format("PlacePicturesManage.aspx?id={0}", place.Id);
                    sitemapdata.Add(new SiteMapDataAttributeNode(place.Name, Request.Url));
                }
                else
                {
                    panelPlaceAltName.Visible = false;
                    LocationSelector.ChangeCountry(sender, e);

                    string type = Request.QueryString["type"];
                    if (!string.IsNullOrEmpty(type))
                    {
                        ListItem i_type = selectType.Items.FindByValue(type);
                        if (i_type != null) i_type.Selected = true;
                    }

                    string name = Request.QueryString["name"];
                    if (!string.IsNullOrEmpty(name)) inputName.Text = name;

                    LocationSelector.ChangeCityWithAccountDefault(sender, new CityLocationEventArgs(Request.QueryString["city"]));

                    linkDelete.Visible = false;
                    linkEditAttributes.Visible = false;
                    linkEditPictures.Visible = false;
                    sitemapdata.Add(new SiteMapDataAttributeNode("New Place", Request.Url));
                }

                StackSiteMap(sitemapdata);
            }

            if (!SessionManager.AccountService.HasVerifiedEmail(SessionManager.Ticket))
            {
                ReportWarning("You don't have any verified e-mail addresses.\n" +
                    "You must add/confirm a valid e-mail address before submitting places.");

                manageAdd.Enabled = false;
            }

            SetDefaultButton(manageAdd);
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
            TransitPlace t = new TransitPlace();
            t.Name = inputName.Text;
            t.Type = selectType.SelectedValue;
            t.Id = RequestId;
            t.Description = inputDescription.Text;
            t.CrossStreet = inputCrossStreet.Text;
            t.Email = inputEmail.Text;
            t.Fax = inputFax.Text;
            t.Phone = inputPhone.Text;
            t.Street = inputStreet.Text;

            if (!string.IsNullOrEmpty(inputWebsite.Text) && !Uri.IsWellFormedUriString(inputWebsite.Text, UriKind.Absolute))
                inputWebsite.Text = "http://" + inputWebsite.Text;

            t.Website = inputWebsite.Text;
            t.Zip = inputZip.Text;
            t.City = inputCity.Text;
            t.Neighborhood = inputNeighborhood.Text;
            t.State = inputState.SelectedValue;
            t.Country = inputCountry.SelectedValue;
            int place_id = SessionManager.PlaceService.CreateOrUpdatePlace(SessionManager.Ticket, t);

            ppg.PlaceId = place_id;
            ppg.save_Click(sender, e);

            Redirect(string.Format("PlaceView.aspx?id={0}", place_id));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridPlaceNamesManage_OnGetDataSource(object sender, EventArgs e)
    {
        gridPlaceNamesManage.DataSource = SessionManager.PlaceService.GetPlaceNames(RequestId);
    }

    public void altname_save_Click(object sender, EventArgs e)
    {
        try
        {
            TransitPlaceName tn = new TransitPlaceName();
            tn.Name = inputAltName.Text;
            tn.PlaceId = RequestId;
            SessionManager.PlaceService.CreateOrUpdatePlaceName(SessionManager.Ticket, tn);
            ReportInfo("Alternate name added.");
            gridPlaceNamesManage_OnGetDataSource(sender, e);
            gridPlaceNamesManage.DataBind();
            inputAltName.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private enum Cells
    {
        id = 0
    }

    public void gridPlaceNamesManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
            switch (e.CommandName)
            {
                case "Delete":
                    SessionManager.PlaceService.DeletePlaceName(SessionManager.Ticket, id);
                    ReportInfo("Alternate place name deleted.");
                    gridPlaceNamesManage.CurrentPageIndex = 0;
                    gridPlaceNamesManage_OnGetDataSource(sender, e);
                    gridPlaceNamesManage.DataBind();
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void linkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            SessionManager.PlaceService.DeletePlace(SessionManager.Ticket, RequestId);
            Redirect("AccountPlacesManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void LocationSelector_StateChanged(object sender, EventArgs e)
    {

    }

    void LocationSelector_CountryChanged(object sender, EventArgs e)
    {
        panelCountryState.Update();
    }

    public void linkLookup_Click(object sender, EventArgs e)
    {
        try
        {
            panelLookup.Update();

            if (string.IsNullOrEmpty(inputName.Text))
            {
                labelLookup.Text = "Please enter a name.";
                return;
            }

            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = 0;
            options.PageSize = 10;
            object[] args = { inputName.Text, options };
            gridLookupPlaces.DataSource = SessionManager.GetCachedCollection<TransitPlace>(
                SessionManager.PlaceService, "SearchPlaces", args);
            gridLookupPlaces.DataBind();

            if (gridLookupPlaces.Items.Count == 0)
            {
                labelLookup.Text = string.Format("No places matching '{0}'.",
                    base.Render(inputName.Text));
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
