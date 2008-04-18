using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class ManagedSecurityContext
    {
        private Account mAccount;

        public Account Account
        {
            get
            {
                return mAccount;
            }
            set
            {
                mAccount = value;
            }
        }

        public ManagedSecurityContext(ISession session)
        {
            mAccount = null;
        }

        public ManagedSecurityContext(ISession session, int id)
        {
            mAccount = session.Load<Account>(id);
        }

        public ManagedSecurityContext(Account value)
        {
            mAccount = value;
        }

        public ManagedSecurityContext(ISession session, string ticket)
        {
            mAccount = null;
            
            try
            {
                if (!string.IsNullOrEmpty(ticket))
                {
                    int id = ManagedAccount.GetAccountIdFromTicket(ticket);
                    mAccount = (id > 0 ? session.Load<Account>(id) : null);
                }
            }
            catch (ManagedAccount.AccessDeniedException)
            {
            }
        }

        public bool IsAdministrator()
        {
            return mAccount != null && mAccount.IsAdministrator;
        }

        public void CheckVerifiedEmail()
        {
            foreach (AccountEmail email in Collection<AccountEmail>.GetSafeCollection(mAccount.AccountEmails))
            {
                if (email.Verified)
                {
                    return;
                }
            }

            throw new ManagedAccount.NoVerifiedEmailException();
        }
    }
}
