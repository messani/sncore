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
using System.Collections.Generic;
using SnCore.Services;

public partial class SiteMap2 : Page
{
    public class PagedListItem
    {
        private int mPageNumber;
        private int mPageSize;
        private int mStart;
        private int mCount;
        private int mPageCount;

        public int PageNumber
        {
            get
            {
                return mPageNumber;
            }
        }

        public int PageSize
        {
            get
            {
                return mPageSize;
            }
        }

        public int Start
        {
            get
            {
                return mStart;
            }
        }

        public int PageCount
        {
            get
            {
                return mPageCount;
            }
        }

        public int Count
        {
            get
            {
                return mCount;
            }
        }

        public PagedListItem(int pagenumber, int pagesize, int start, int count, int pagecount)
        {
            mPageNumber = pagenumber;
            mPageSize = pagesize;
            mStart = start;
            mCount = count;
            mPageCount = pagecount;
        }
    }

    public List<PagedListItem> GetPagedList(int count)
    {
        const int segment = 100;
        int pagecount = count / segment + 1;
        List<PagedListItem> result = new List<PagedListItem>(pagecount);
        int current = 1;
        int currentpage = 0;
        while (current <= count)
        {
            int delta = count - current + 1;
            result.Add(new PagedListItem(currentpage++, segment, current, (delta > segment) ? segment : delta, pagecount));
            current += segment;
        }
        return result;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            linkSuggestFeature.HRef =
                string.Format("BugEdit.aspx?pid={0}&type=Suggestion",
                    SystemService.GetConfigurationByNameWithDefault(
                        "SnCore.NewFeature.ProjectId", "0").Value);

            linkReportBug.HRef =
                string.Format("BugEdit.aspx?pid={0}&type=Bug",
                    SystemService.GetConfigurationByNameWithDefault(
                        "SnCore.Bug.ProjectId", "0").Value);

            linkSiteDiscussion.HRef =
                string.Format("DiscussionView.aspx?id={0}",
                    SystemService.GetConfigurationByNameWithDefault(
                        "SnCore.Site.DiscussionId", "0").Value);

            linkBlog.HRef = 
                string.Format("AccountBlogView.aspx?id={0}",
                    SystemService.GetConfigurationByNameWithDefault(
                        "SnCore.Blog.Id", "0").Value);

            AccountActivityQueryOptions aaqo = new AccountActivityQueryOptions();
            listAccounts.DataSource = GetPagedList(SocialService.GetAccountActivityCount(aaqo));
            listAccounts.DataBind();

            listFeeds.DataSource = GetPagedList(SyndicationService.GetUpdatedAccountFeedsCount());
            listFeeds.DataBind();

            TransitPlaceQueryOptions pqo = new TransitPlaceQueryOptions();
            listPlaces.DataSource = GetPagedList(PlaceService.GetPlacesCount(pqo));
            listPlaces.DataBind();

            TransitAccountEventQueryOptions aeqo = new TransitAccountEventQueryOptions();
            listAccountEvents.DataSource = GetPagedList(EventService.GetAccountEventsCount(aeqo));
            listAccountEvents.DataBind();

            listStories.DataSource = GetPagedList(StoryService.GetLatestAccountStoriesCount());
            listStories.DataBind();

            listDiscussions.DataSource = DiscussionService.GetDiscussions();
            listDiscussions.DataBind();

            listSurveys.DataSource = SystemService.GetSurveys();
            listSurveys.DataBind();
        }
    }
}