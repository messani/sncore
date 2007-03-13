using System;
using NHibernate;
using System.Collections;
using System.IO;
using SnCore.Tools.Drawing;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountGroupPicture : TransitArrayElementService<AccountGroupPicture>
    {
        private byte[] mThumbnail;

        public byte[] Thumbnail
        {
            get
            {
                return mThumbnail;
            }
            set
            {
                mThumbnail = value;
            }
        }

        private byte[] mBitmap;

        public byte[] Bitmap
        {
            get
            {
                return mBitmap;
            }
            set
            {
                mBitmap = value;
            }
        }

        private string mName;

        public string Name
        {
            get
            {

                return mName;
            }
            set
            {
                mName = value;
            }
        }

        private string mDescription;

        public string Description
        {
            get
            {

                return mDescription;
            }
            set
            {
                mDescription = value;
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

        private int mAccountGroupId;

        public int AccountGroupId
        {
            get
            {

                return mAccountGroupId;
            }
            set
            {
                mAccountGroupId = value;
            }
        }

        private int mCommentCount;

        public int CommentCount
        {
            get
            {

                return mCommentCount;
            }
            set
            {
                mCommentCount = value;
            }
        }

        private TransitCounter mCounter;

        public TransitCounter Counter
        {
            get
            {
                return mCounter;
            }
            set
            {
                mCounter = value;
            }
        }

        private int mAccountId;

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

        private string mAccountName;

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

        public TransitAccountGroupPicture()
        {

        }

        public TransitAccountGroupPicture(AccountGroupPicture p)
            : base(p)
        {

        }

        public override void SetInstance(AccountGroupPicture instance)
        {
            base.SetInstance(instance);
            Name = instance.Name;
            Description = instance.Description;
            Created = instance.Created;
            Modified = instance.Modified;
            AccountGroupId = instance.AccountGroup.Id;
            AccountId = instance.Account.Id;
            AccountName = instance.Account.Name;
            Bitmap = instance.Bitmap;
            Thumbnail = new ThumbnailBitmap(Bitmap).Thumbnail;
        }

        public override AccountGroupPicture GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountGroupPicture p = base.GetInstance(session, sec);
            p.Name = this.Name;
            p.Description = this.Description;
            if (Id == 0)
            {
                if (AccountGroupId > 0) p.AccountGroup = session.Load<AccountGroup>(AccountGroupId);
                p.Account = GetOwner(session, AccountId, sec);
            }
            if (Bitmap != null) p.Bitmap = Bitmap;
            return p;
        }
    }

    /// <summary>
    /// Managed AccountGroup picture.
    /// </summary>
    public class ManagedAccountGroupPicture : ManagedService<AccountGroupPicture, TransitAccountGroupPicture>
    {
        public ManagedAccountGroupPicture()
        {

        }

        public ManagedAccountGroupPicture(ISession session)
            : base(session)
        {

        }

        public ManagedAccountGroupPicture(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountGroupPicture(ISession session, AccountGroupPicture value)
            : base(session, value)
        {

        }
       
        public void MigrateToAccount(Account newowner)
        {
            mInstance.Account = newowner;
            Session.Save(mInstance);
        }

        public override TransitAccountGroupPicture GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitAccountGroupPicture t_instance = base.GetTransitInstance(sec);
            t_instance.SetWithinCollection(mInstance, mInstance.AccountGroup.AccountGroupPictures);
            t_instance.CommentCount = ManagedDiscussion.GetDiscussionPostCount(
                Session, mInstance.Account.Id,
                typeof(AccountGroupPicture), mInstance.Id);
            t_instance.Counter = ManagedStats.FindByUri(Session, "AccountGroupPicture.aspx", mInstance.Id, sec);
            return t_instance;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            // everyone is able to see this membership if the group is public
            if (!mInstance.AccountGroup.IsPrivate) acl.Add(new ACLEveryoneAllowRetrieve());
            // the user who has uploaded the picture can do anything to it
            if (mInstance.Account != null) acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            // members can create or see the pictures depending on their permissions
            foreach (AccountGroupAccount account in Collection<AccountGroupAccount>.GetSafeCollection(mInstance.AccountGroup.AccountGroupAccounts))
            {
                acl.Add(new ACLAccount(account.Account, account.IsAdministrator
                    ? DataOperation.All
                    : DataOperation.Retreive | DataOperation.Create));
            }
            return acl;
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            ManagedDiscussion.FindAndDelete(Session, mInstance.Account.Id, typeof(AccountGroupPicture), mInstance.Id, sec);
            base.Delete(sec);
        }

        protected override void Check(TransitAccountGroupPicture t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0) sec.CheckVerifiedEmail();
        }
    }
}