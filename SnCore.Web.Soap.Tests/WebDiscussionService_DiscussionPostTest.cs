using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using System.Threading;

namespace SnCore.Web.Soap.Tests.WebDiscussionServiceTests
{
    [TestFixture]
    public class DiscussionPostTest : WebServiceTest<WebDiscussionService.TransitDiscussionPost, WebDiscussionServiceNoCache>
    {
        public DiscussionTest _discussion = new DiscussionTest();
        public int _discussion_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _discussion.SetUp();
            _discussion_id = _discussion.Create(GetAdminTicket());
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _discussion.Delete(GetAdminTicket(), _discussion_id);
            _discussion.TearDown();
        }

        public DiscussionPostTest()
            : base("DiscussionPost")
        {

        }

        public override WebDiscussionService.TransitDiscussionPost GetTransitInstance()
        {
            WebDiscussionService.TransitDiscussionPost t_instance = new WebDiscussionService.TransitDiscussionPost();
            t_instance.Body = GetNewString();
            t_instance.DiscussionId = _discussion_id;
            t_instance.Subject = GetNewString();
            t_instance.Sticky = false;
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _discussion_id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _discussion_id };
            return args;
        }

        [Test]
        public void GetLatestDiscussionPostsTest()
        {
            WebDiscussionService.TransitDiscussionPost t_post = GetTransitInstance();
            t_post.Id = Create(GetAdminTicket(), t_post);
            WebDiscussionService.ServiceQueryOptions options = new WebDiscussionService.ServiceQueryOptions();
            options.PageNumber = 0;
            options.PageSize = 25;
            WebDiscussionService.TransitDiscussionPost[] posts = EndPoint.GetLatestDiscussionPosts(GetUserTicket(), null);
            Assert.IsTrue(posts.Length > 0);
            Console.WriteLine("Posts: {0}", posts.Length);
            Assert.IsTrue(new TransitServiceCollection<WebDiscussionService.TransitDiscussionPost>(posts).ContainsId(t_post.Id));
            Delete(GetAdminTicket(), t_post.Id);
        }

        [Test]
        public void GetLatestDiscussionPostsByIdTest()
        {
            WebDiscussionService.TransitDiscussionPost t_post = GetTransitInstance();
            t_post.Id = Create(GetAdminTicket(), t_post);
            WebDiscussionService.ServiceQueryOptions options = new WebDiscussionService.ServiceQueryOptions();
            options.PageNumber = 0;
            options.PageSize = 25;
            WebDiscussionService.TransitDiscussionPost[] posts = EndPoint.GetLatestDiscussionPostsById(
                GetUserTicket(), t_post.DiscussionId, null);
            Assert.IsTrue(posts.Length > 0);
            Console.WriteLine("Posts: {0}", posts.Length);
            Assert.IsTrue(new TransitServiceCollection<WebDiscussionService.TransitDiscussionPost>(posts).ContainsId(t_post.Id));
            Delete(GetAdminTicket(), t_post.Id);
        }

        [Test]
        public void SearchDiscussionPostsEmptyTest()
        {
            WebDiscussionService.TransitDiscussionPost[] posts = EndPoint.SearchDiscussionPosts(
                GetUserTicket(), string.Empty, null);
            Assert.AreEqual(0, posts.Length);
        }

        [Test]
        public void SearchDiscussionPostsTest()
        {
            WebDiscussionService.TransitDiscussionPost t_post = GetTransitInstance();
            t_post.Id = Create(GetAdminTicket(), t_post);
            Thread.Sleep(2000); // wait for reindex
            WebDiscussionService.ServiceQueryOptions options = new WebDiscussionService.ServiceQueryOptions();
            options.PageNumber = 0;
            options.PageSize = 25;
            WebDiscussionService.TransitDiscussionPost[] posts = EndPoint.SearchDiscussionPosts(
                GetUserTicket(), t_post.Subject, null);
            Assert.IsTrue(posts.Length > 0);
            Console.WriteLine("Posts: {0}", posts.Length);
            Assert.IsTrue(new TransitServiceCollection<WebDiscussionService.TransitDiscussionPost>(posts).ContainsId(t_post.Id));
            Delete(GetAdminTicket(), t_post.Id);
        }

        [Test]
        public void SearchDiscussionPostsByIdTest()
        {
            WebDiscussionService.TransitDiscussionPost t_post = GetTransitInstance();
            t_post.Id = Create(GetAdminTicket(), t_post);
            Thread.Sleep(2000); // wait for reindex
            WebDiscussionService.ServiceQueryOptions options = new WebDiscussionService.ServiceQueryOptions();
            options.PageNumber = 0;
            options.PageSize = 25;
            WebDiscussionService.TransitDiscussionPost[] posts = EndPoint.SearchDiscussionPostsById(
                GetUserTicket(), t_post.DiscussionId, t_post.Subject, null);
            Assert.IsTrue(posts.Length > 0);
            Console.WriteLine("Posts: {0}", posts.Length);
            Assert.IsTrue(new TransitServiceCollection<WebDiscussionService.TransitDiscussionPost>(posts).ContainsId(t_post.Id));
            Delete(GetAdminTicket(), t_post.Id);
        }

        [Test]
        public void DiscussionPostQuotaTest()
        {
            UserInfo user = CreateUserWithVerifiedEmailAddress();

            List<int> ids = new List<int>();
            int limit = 30; // ManagedDiscussionPost.DefaultHourlyLimit

            for (int i = 0; i < limit; i++)
            {
                WebDiscussionService.TransitDiscussionPost t_post = GetTransitInstance();
                t_post.AccountId = user.id;
                t_post.Id = EndPoint.CreateOrUpdateDiscussionPost(user.ticket, t_post);
                Console.WriteLine("{0}: Post: {1}", i, t_post.Id);
                ids.Add(t_post.Id);
            }

            try
            {
                WebDiscussionService.TransitDiscussionPost t_post = GetTransitInstance();
                t_post.AccountId = user.id;
                t_post.Id = EndPoint.CreateOrUpdateDiscussionPost(user.ticket, t_post);
                Console.WriteLine("Post: {0}", t_post.Id);
                Assert.IsTrue(false, "Expected a quota exceeded exception.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Expected exception: {0}", ex.Message);
                Assert.IsTrue(ex.Message.StartsWith("System.Web.Services.Protocols.SoapException: Server was unable to process request. ---> SnCore.Services.ManagedAccount+QuotaExceededException: Quota exceeded"));
            }

            // delete all these

            foreach (int id in ids)
            {
                EndPoint.DeleteDiscussionPost(GetAdminTicket(), id);
            }

            DeleteUser(user.id);
        }

        [Test]
        public void DiscussionPostStickynessAdminTest()
        {
            // check that the system admin can create and flip the stickyness of the post
            WebDiscussionService.TransitDiscussionPost t_post = GetTransitInstance();
            t_post.Id = EndPoint.CreateOrUpdateDiscussionPost(GetAdminTicket(), t_post);
            Console.WriteLine("Post: {0}", t_post.Id);
            WebDiscussionService.TransitDiscussionPost t_post_copy = EndPoint.GetDiscussionPostById(GetAdminTicket(), t_post.Id);
            Assert.AreEqual(t_post.Id, t_post_copy.Id);
            Assert.AreEqual(false, t_post_copy.Sticky);
            Assert.AreEqual(t_post.Sticky, t_post_copy.Sticky);
            t_post.Sticky = true;
            t_post.Id = EndPoint.CreateOrUpdateDiscussionPost(GetAdminTicket(), t_post);
            t_post_copy = EndPoint.GetDiscussionPostById(GetAdminTicket(), t_post.Id);
            Assert.AreEqual(t_post.Id, t_post_copy.Id);
            Assert.AreEqual(true, t_post_copy.Sticky);
            Assert.AreEqual(t_post.Sticky, t_post_copy.Sticky);            
            t_post.Sticky = false;
            t_post.Id = EndPoint.CreateOrUpdateDiscussionPost(GetAdminTicket(), t_post);
            t_post_copy = EndPoint.GetDiscussionPostById(GetAdminTicket(), t_post.Id);
            Assert.AreEqual(t_post.Id, t_post_copy.Id);
            Assert.AreEqual(false, t_post_copy.Sticky);
            Assert.AreEqual(t_post.Sticky, t_post_copy.Sticky);
            EndPoint.DeleteDiscussionPost(GetAdminTicket(), t_post.Id);
        }

        [Test]
        public void DiscussionPostStickynessDiscussionOwnerTest()
        {
            WebDiscussionService.TransitDiscussion t_discussion = new WebDiscussionService.TransitDiscussion();
            t_discussion.Name = GetNewString();
            t_discussion.Personal = false;
            t_discussion.Description = GetNewString();
            t_discussion.Id = EndPoint.CreateOrUpdateDiscussion(GetUserTicket(), t_discussion);
            // check that the discussion admin can create and flip the stickyness of the post
            WebDiscussionService.TransitDiscussionPost t_post = GetTransitInstance();
            t_post.DiscussionId = t_discussion.Id;
            t_post.AccountId = GetUserAccount().Id;
            t_post.Id = EndPoint.CreateOrUpdateDiscussionPost(GetUserTicket(), t_post);
            Console.WriteLine("Post: {0}", t_post.Id);
            WebDiscussionService.TransitDiscussionPost t_post_copy = EndPoint.GetDiscussionPostById(GetUserTicket(), t_post.Id);
            Assert.AreEqual(t_post.Id, t_post_copy.Id);
            Assert.AreEqual(false, t_post_copy.Sticky);
            Assert.AreEqual(t_post.Sticky, t_post_copy.Sticky);
            t_post.Sticky = true;
            // check that a regular user cannot flip the post stickyness
            UserInfo user = CreateUserWithVerifiedEmailAddress();
            try
            {
                EndPoint.CreateOrUpdateDiscussionPost(user.ticket, t_post);
                Assert.IsTrue(false, "Expected an access denied.");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Expected exception: {0}", ex.Message);
                Assert.IsTrue(ex.Message.StartsWith("System.Web.Services.Protocols.SoapException: Server was unable to process request. ---> SnCore.Services.ManagedAccount+AccessDeniedException: Access denied"));
            }
            // check the that discussion admin can
            t_post.Id = EndPoint.CreateOrUpdateDiscussionPost(GetUserTicket(), t_post);
            t_post_copy = EndPoint.GetDiscussionPostById(GetUserTicket(), t_post.Id);
            Assert.AreEqual(t_post.Id, t_post_copy.Id);
            Assert.AreEqual(true, t_post_copy.Sticky);
            Assert.AreEqual(t_post.Sticky, t_post_copy.Sticky);
            t_post.Sticky = false;
            try
            {
                EndPoint.CreateOrUpdateDiscussionPost(user.ticket, t_post);
                Assert.IsTrue(false, "Expected an access denied.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Expected exception: {0}", ex.Message);
                Assert.IsTrue(ex.Message.StartsWith("System.Web.Services.Protocols.SoapException: Server was unable to process request. ---> SnCore.Services.ManagedAccount+AccessDeniedException: Access denied"));
            }
            t_post.Id = EndPoint.CreateOrUpdateDiscussionPost(GetUserTicket(), t_post);
            t_post_copy = EndPoint.GetDiscussionPostById(GetUserTicket(), t_post.Id);
            Assert.AreEqual(t_post.Id, t_post_copy.Id);
            Assert.AreEqual(false, t_post_copy.Sticky);
            Assert.AreEqual(t_post.Sticky, t_post_copy.Sticky);
            EndPoint.DeleteDiscussionPost(GetUserTicket(), t_post.Id);
            EndPoint.DeleteDiscussion(GetUserTicket(), t_discussion.Id);
            DeleteUser(user.id);
        }
    }
}
