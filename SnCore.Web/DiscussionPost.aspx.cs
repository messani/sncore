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
using System.Text;
using Wilco.Web.UI.WebControls;
using SnCore.Tools.Drawing;
using System.IO;
using System.Drawing;
using SnCore.Tools;
using SnCore.SiteMap;
using SnCore.Data.Hibernate;

public partial class DiscussionPostNew : AuthenticatedPage
{
    public DiscussionPostNew()
    {
        mIsMobileEnabled = true;
    }

    public int PostId
    {
        get
        {
            return RequestId;
        }
    }

    public int DiscussionId
    {
        get
        {
            return GetId("did");
        }
    }

    public int ParentId
    {
        get
        {
            return GetId("pid");
        }
    }

    public bool Quote
    {
        get
        {
            object result = Request["Quote"];
            if (result != null) return bool.Parse(result.ToString());
            return false;
        }
    }

    public string ReturnUrl
    {
        get
        {
            string result = Request.Params["ReturnUrl"];
            if (string.IsNullOrEmpty(result) && (DiscussionId > 0)) result = string.Format("DiscussionView.aspx?id={0}", DiscussionId);
            return result;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DomainClass cs = SessionManager.GetDomainClass("DiscussionPost");
            inputSubject.MaxLength = cs["Subject"].MaxLengthInChars;

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();

            this.addFile.Attributes["onclick"] = this.files.GetAddFileScriptReference() + "return false;";

            linkCancel.NavigateUrl = ReturnUrl;

            if (DiscussionId > 0)
            {
                TransitDiscussion td = SessionManager.GetPrivateInstance<TransitDiscussion, int>(
                    DiscussionId, SessionManager.DiscussionService.GetDiscussionById);

                inputSticky.Enabled = td.CanUpdate;

                if (!string.IsNullOrEmpty(td.ParentObjectName))
                {
                    sitemapdata.Add(new SiteMapDataAttributeNode(td.ParentObjectName, Request, td.ParentObjectUri));
                    discussionLabelLink.Text = Renderer.Render(td.ParentObjectName);
                    discussionLabelLink.NavigateUrl = td.ParentObjectUri;
                }
                else
                {
                    discussionLabelLink.Text = Renderer.Render(td.Name);
                    discussionLabelLink.NavigateUrl = string.Format("DiscussionView.aspx?id={0}", td.Id);
                    sitemapdata.Add(new SiteMapDataAttributeNode("Discussions", Request, "DiscussionsView.aspx"));
                    sitemapdata.Add(new SiteMapDataAttributeNode(td.Name, Request, string.Format("DiscussionView.aspx?id={0}", td.Id)));
                }
            }

            StringBuilder body = new StringBuilder();

            if (PostId > 0)
            {
                TransitDiscussionPost tw = SessionManager.DiscussionService.GetDiscussionPostById(SessionManager.Ticket, PostId);
                titleNewPost.Text = Renderer.Render(tw.Subject);
                inputSubject.Text = tw.Subject;
                inputSticky.Checked = tw.Sticky;
                body.Append(tw.Body);
                sitemapdata.Add(new SiteMapDataAttributeNode(tw.Subject, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Post", Request.Url));
            }

            if (ParentId > 0)
            {
                TransitDiscussionPost rp = SessionManager.DiscussionService.GetDiscussionPostById(SessionManager.Ticket, ParentId);
                panelReplyTo.Visible = true;
                replytoSenderName.NavigateUrl = accountlink.HRef = "AccountView.aspx?id=" + rp.AccountId.ToString();
                replytoSenderName.Text = Renderer.Render(rp.AccountName);
                replyToBody.Text = base.RenderEx(rp.Body);
                replytoCreated.Text = SessionManager.ToAdjustedString(rp.Created);
                replytoImage.ImageUrl = string.Format("AccountPictureThumbnail.aspx?id={0}&width=75&height=75", rp.AccountPictureId);
                replytoSubject.Text = Renderer.Render(rp.Subject);
                inputSubject.Text = rp.Subject.StartsWith("Re:") ? rp.Subject : "Re: " + rp.Subject;
                rowsubject.Attributes["style"] = "display: none;";

                if (Quote)
                {
                    body.AppendFormat("<P>[quote]<DIV>on {0} {1} wrote:</DIV><DIV>{2}</DIV>[/quote]</P>",
                        rp.Created.ToString("d"), rp.AccountName, rp.Body);
                }
            }

            if ((PostId == 0) && !string.IsNullOrEmpty(SessionManager.Account.Signature))
            {
                body.Append("<BR /><BR />");
                body.Append("<P>");
                body.Append(Renderer.RenderEx(SessionManager.Account.Signature));
                body.Append("</P>");
            }

            inputBody.Content = body.ToString();

            StackSiteMap(sitemapdata);
        }

        if (!SessionManager.HasVerified())
        {
            ReportWarning("You don't have any verified e-mail addresses and/or profile photos.\n" +
                "You must add/confirm a valid e-mail address and upload a profile photo before posting.");

            panelPost.Visible = false;
            post.Enabled = false;
        }

        SetDefaultButton(post);
    }

    public void post_Click(object sender, EventArgs e)
    {
        TransitDiscussionPost tw = new TransitDiscussionPost();
        tw.Subject = inputSubject.Text;
        if (string.IsNullOrEmpty(tw.Subject)) tw.Subject = "Untitled";
        tw.Body = inputBody.Content;
        tw.Id = PostId;
        tw.DiscussionPostParentId = ParentId;
        tw.DiscussionId = DiscussionId;
        if (inputSticky.Enabled) tw.Sticky = inputSticky.Checked;
        SessionManager.CreateOrUpdate<TransitDiscussionPost>(
            tw, SessionManager.DiscussionService.CreateOrUpdateDiscussionPost);
        SessionManager.InvalidateCache<TransitDiscussion>();
        SessionManager.InvalidateCache<TransitDiscussionThread>();
        Redirect(linkCancel.NavigateUrl);
    }

    protected void files_FilesPosted(object sender, FilesPostedEventArgs e)
    {
        try
        {
            if (e.PostedFiles.Count == 0)
                return;

            ExceptionCollection exceptions = new ExceptionCollection();
            foreach (HttpPostedFile file in e.PostedFiles)
            {
                try
                {
                    TransitAccountPicture p = new TransitAccountPicture();

                    ThumbnailBitmap t = new ThumbnailBitmap(file.InputStream);
                    p.Bitmap = t.Bitmap;
                    p.Name = Path.GetFileName(file.FileName);
                    p.Description = string.Empty;
                    p.Hidden = true;

                    int id = SessionManager.CreateOrUpdate<TransitAccountPicture>(
                        p, SessionManager.AccountService.CreateOrUpdateAccountPicture);

                    Size size = t.GetNewSize(new Size(200, 200));

                    inputBody.Content = string.Format("<a href=AccountPictureView.aspx?id={2}><img border=0 width={0} height={1} src=AccountPicture.aspx?id={2}></a>\n{3}",
                        size.Width, size.Height, id, inputBody.Content);
                }
                catch (Exception ex)
                {
                    exceptions.Add(new Exception(string.Format("Error processing {0}: {1}",
                        Renderer.Render(file.FileName), ex.Message), ex));
                }
            }
            exceptions.Throw();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
