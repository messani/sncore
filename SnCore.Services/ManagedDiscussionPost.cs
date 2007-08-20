using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Resources;
using System.Net.Mail;
using System.IO;
using System.Collections.Generic;
using SnCore.Data.Hibernate;
using SnCore.Tools.Web;

namespace SnCore.Services
{
    public class TransitDiscussionPost : TransitService<DiscussionPost>
    {
        private bool mCanEdit = false;

        public bool CanEdit
        {
            get
            {

                return mCanEdit;
            }
            set
            {
                mCanEdit = value;
            }
        }

        private bool mCanDelete = false;

        public bool CanDelete
        {
            get
            {

                return mCanDelete;
            }
            set
            {
                mCanDelete = value;
            }
        }

        private DateTime mCreated;

        public DateTime Created
        {
            get
            {

                return mCreated;
            }
            set
            {
                mCreated = value;
            }
        }

        private DateTime mModified;

        public DateTime Modified
        {
            get
            {

                return mModified;
            }
            set
            {
                mModified = value;
            }
        }

        private int mDiscussionId = 0;

        public int DiscussionId
        {
            get
            {

                return mDiscussionId;
            }
            set
            {
                mDiscussionId = value;
            }
        }

        private string mDiscussionName;

        public string DiscussionName
        {
            get
            {

                return mDiscussionName;
            }
            set
            {
                mDiscussionName = value;
            }
        }

        private int mDiscussionThreadId = 0;

        public int DiscussionThreadId
        {
            get
            {

                return mDiscussionThreadId;
            }
            set
            {
                mDiscussionThreadId = value;
            }
        }

        private int mInstanceParentId = 0;

        public int DiscussionPostParentId
        {
            get
            {

                return mInstanceParentId;
            }
            set
            {
                mInstanceParentId = value;
            }
        }

        private int mAccountId = 0;

        public int AccountId
        {
            get
            {

                return mAccountId;
            }
            set
            {
                mAccountId = value;
            }
        }

        private string mAccountName = string.Empty;

        public string AccountName
        {
            get
            {

                return mAccountName;
            }
            set
            {
                mAccountName = value;
            }
        }

        private int mAccountPictureId = 0;

        public int AccountPictureId
        {
            get
            {

                return mAccountPictureId;
            }
            set
            {
                mAccountPictureId = value;
            }
        }

        private string mBody = string.Empty;

        public string Body
        {
            get
            {

                return mBody;
            }
            set
            {
                mBody = value;
            }
        }

        private string mSubject = string.Empty;

        public string Subject
        {
            get
            {

                return mSubject;
            }
            set
            {
                mSubject = value;
            }
        }

        private int mLevel = 0;

        public int Level
        {
            get
            {

                return mLevel;
            }
            set
            {
                mLevel = value;
            }
        }

        private DateTime mDiscussionThreadModified;

        public DateTime DiscussionThreadModified
        {
            get
            {

                return mDiscussionThreadModified;
            }
            set
            {
                mDiscussionThreadModified = value;
            }
        }

        private int mDiscussionThreadCount = 0;

        public int DiscussionThreadCount
        {
            get
            {

                return mDiscussionThreadCount;
            }
            set
            {
                mDiscussionThreadCount = value;
            }
        }

        private int mRepliesCount = 0;

        public int RepliesCount
        {
            get
            {

                return mRepliesCount;
            }
            set
            {
                mRepliesCount = value;
            }
        }

        public TransitDiscussionPost()
        {

        }

        public override DiscussionPost GetInstance(ISession session, ManagedSecurityContext sec)
        {
            DiscussionPost instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                if (DiscussionPostParentId > 0)
                {
                    instance.DiscussionPostParent = session.Load<DiscussionPost>(this.DiscussionPostParentId);
                    instance.DiscussionThread = instance.DiscussionPostParent.DiscussionThread;
                }
                else if (DiscussionThreadId > 0)
                {
                    instance.DiscussionThread = session.Load<DiscussionThread>(this.DiscussionThreadId);
                }
                else
                {
                    instance.DiscussionThread = new DiscussionThread();
                    instance.DiscussionThread.Discussion = session.Load<Discussion>(this.DiscussionId);
                    instance.DiscussionThread.Created = instance.DiscussionThread.Modified = DateTime.UtcNow;
                }

                if (DiscussionThreadId > 0 && instance.DiscussionThread.Id != DiscussionThreadId)
                    throw new ArgumentException("Invalid Thread Id");

                if (DiscussionId > 0 && instance.DiscussionThread.Discussion.Id != DiscussionId)
                    throw new ArgumentException("Invalid Discussion Id");

                instance.AccountId = GetOwner(session, AccountId, sec).Id;
            }

            instance.Body = this.Body;
            instance.Subject = this.Subject;
            return instance;
        }

        public override void SetInstance(DiscussionPost instance)
        {
            DiscussionName = instance.DiscussionThread.Discussion.Name;
            DiscussionId = instance.DiscussionThread.Discussion.Id;
            DiscussionThreadId = instance.DiscussionThread.Id;
            DiscussionThreadModified = instance.DiscussionThread.Modified;
            DiscussionThreadCount =
                (instance.DiscussionThread.DiscussionPosts != null) ?
                    instance.DiscussionThread.DiscussionPosts.Count :
                    0;
            RepliesCount =
                (instance.DiscussionPosts != null) ?
                    instance.DiscussionPosts.Count :
                    0;
            DiscussionPostParentId = (instance.DiscussionPostParent == null) ? 0 : instance.DiscussionPostParent.Id;
            AccountId = instance.AccountId;
            Body = instance.Body;
            Subject = instance.Subject;
            Created = instance.Created;
            Modified = instance.Modified;
            Level = 0;
            DiscussionPost parent = instance.DiscussionPostParent;
            while (parent != null && parent != instance)
            {
                Level++;
                parent = parent.DiscussionPostParent;
            }

            base.SetInstance(instance);
        }
    }

    public class ManagedDiscussionPost : ManagedService<DiscussionPost, TransitDiscussionPost>
    {
        public ManagedDiscussionPost()
        {

        }

        public ManagedDiscussionPost(ISession session)
            : base(session)
        {

        }

        public ManagedDiscussionPost(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedDiscussionPost(ISession session, DiscussionPost value)
            : base(session, value)
        {

        }

        public int DiscussionId
        {
            get
            {
                return mInstance.DiscussionThread.Discussion.Id;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            foreach (DiscussionPost post in Collection<DiscussionPost>.GetSafeCollection(mInstance.DiscussionPosts))
            {
                new ManagedDiscussionPost(Session, post).Delete(sec);
            }

            mInstance.DiscussionPosts = null;
            Collection<DiscussionPost>.GetSafeCollection(mInstance.DiscussionThread.DiscussionPosts).Remove(mInstance);

            base.Delete(sec);
        }

        public override TransitDiscussionPost GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitDiscussionPost post = base.GetTransitInstance(sec);
            post.AccountName = ManagedAccount.GetAccountNameWithDefault(Session, post.AccountId);
            post.AccountPictureId = ManagedAccount.GetRandomAccountPictureId(Session, post.AccountId);
            post.CanEdit = (GetACL().Apply(sec, DataOperation.Update) == ACLVerdict.Allowed);
            post.CanDelete = (GetACL().Apply(sec, DataOperation.Delete) == ACLVerdict.Allowed);
            return post;
        }

        public List<TransitDiscussionPost> GetPosts(ManagedSecurityContext sec)
        {
            if (mInstance.DiscussionPosts == null)
                return new List<TransitDiscussionPost>();

            List<TransitDiscussionPost> result = new List<TransitDiscussionPost>(mInstance.DiscussionPosts.Count);
            foreach (DiscussionPost post in Collection<DiscussionPost>.GetSafeCollection(mInstance.DiscussionPosts))
            {
                ManagedDiscussionPost m_post = new ManagedDiscussionPost(Session, post);
                result.Insert(0, m_post.GetTransitInstance(sec));
                result.InsertRange(1, m_post.GetPosts(sec));
            }

            return result;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            DateTime lastModified = mInstance.Modified;
            mInstance.Modified = DateTime.UtcNow;

            // message cannot span discussions
            if (mInstance.DiscussionPostParent != null &&
                mInstance.DiscussionPostParent.DiscussionThread.Discussion.Id != mInstance.DiscussionThread.Discussion.Id)
            {
                throw new ArgumentException("Invalid Discussion Id");
            }

            if (mInstance.Id == 0)
            {
                // both discussion thread and discussion board are updated to simplify queries
                mInstance.Created = mInstance.Modified;
                mInstance.DiscussionThread.Modified = mInstance.Modified;
                mInstance.DiscussionThread.Discussion.Modified = mInstance.Modified;
                Session.Save(mInstance.DiscussionThread);
            }
            base.Save(sec);
            Session.Flush();

            try
            {
                ManagedAccount ra = new ManagedAccount(Session, mInstance.AccountId);
                ManagedAccount ma = new ManagedAccount(Session, mInstance.DiscussionPostParent != null
                    ? mInstance.DiscussionPostParent.AccountId
                    : mInstance.DiscussionThread.Discussion.Account.Id);

                // if the author is editing the post, don't notify within 30 minute periods
                if (ra.Id != ma.Id && (mInstance.Id == 0 || lastModified.AddMinutes(30) > DateTime.UtcNow))
                {
                    Session.Flush();

                    ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(
                        Session, ma, string.Format("EmailDiscussionPost.aspx?id={0}", mInstance.Id));
                }
            }
            catch (ObjectNotFoundException)
            {
                // replying to an account that does not exist
            }
        }

        public override ACL GetACL(Type type)
        {
            ManagedDiscussionMapEntry mapentry = null;
            ACL acl = null;

            if (mInstance.DiscussionThread.Discussion.Personal &&
                ManagedDiscussionMap.TryFind(mInstance.DiscussionThread.Discussion.Name, out mapentry))
            {
                acl = mapentry.GetACL(Session, mInstance.DiscussionThread.Discussion.ObjectId, typeof(DiscussionPost));
                if (mInstance.Id > 0) acl.Add(new ACLAccountId(mInstance.AccountId, DataOperation.All));
            }
            else
            {
                acl = base.GetACL(type);
                acl.Add(new ACLEveryoneAllowRetrieve());
                acl.Add(new ACLAuthenticatedAllowCreate());
                acl.Add(new ACLAccountId(mInstance.AccountId, DataOperation.All));
            }

            return acl;
        }

        public const int DefaultHourlyLimit = 30; // TODO: export into configuration settings

        // messages sent by this user to those who aren't this user's friends
        public static IList<DiscussionPost> GetDiscussionPosts(ISession session, int account_id, DateTime limit)
        {
            return session.CreateQuery("FROM DiscussionPost p" +
                string.Format(" WHERE p.AccountId = {0}", account_id) +
                string.Format(" AND p.Created >= '{0}'", limit)
                ).List<DiscussionPost>();
        }

        protected override void Check(TransitDiscussionPost t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);

            if (t_instance.Id != 0)
                return;

            sec.CheckVerifiedEmail();

            int account_id = t_instance.GetOwner(Session, t_instance.AccountId, sec).Id;

            try
            {
                // how many posts within the last hour?
                new ManagedQuota(DefaultHourlyLimit).Check<DiscussionPost, ManagedAccount.QuotaExceededException>(
                    GetDiscussionPosts(Session, account_id, DateTime.UtcNow.AddHours(-1)));
                // how many messages within the last 24 hours?
                ManagedQuota.GetDefaultEnabledQuota().Check<DiscussionPost, ManagedAccount.QuotaExceededException>(
                    GetDiscussionPosts(Session, account_id, DateTime.UtcNow.AddDays(-1)));
            }
            catch (ManagedAccount.QuotaExceededException)
            {
                ManagedAccount admin = new ManagedAccount(Session, ManagedAccount.GetAdminAccount(Session));
                ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(
                    Session, admin,
                    string.Format("EmailAccountQuotaExceeded.aspx?id={0}", account_id));
                throw;
            }

            // check whether the sender was flagged
            new ManagedQuota(ManagedAccountFlag.DefaultAccountFlagThreshold).Check<AccountFlag, ManagedAccountFlag.AccountFlaggedException>(
                ManagedAccountFlag.GetAccountFlagsByFlaggedAccountId(Session, sec.Account.Id));
        }
    }
}
