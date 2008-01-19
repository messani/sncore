using System;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using Microsoft.Exchange.Transport.EventInterop;
using Microsoft.Exchange.Transport.EventWrappers;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Text;
using MIME;
using System.Collections;
using System.Collections.Generic;
using SnCore.Data;
using SnCore.Data.Hibernate;
using NHibernate;
using NHibernate.Expression;

namespace SnCore.DomainMail
{
    [Guid("E6A74E11-7234-4096-B9E6-C65165B9B93F")]
    [ComVisible(true)]
    public class Sink : IMailTransportSubmission
    {
        private static bool s_Debug = true;
        private static FileSystemWatcher s_ConfigurationChangeWatcher = null;

        static Sink()
        {
            LoadConfiguration();
        }

        public Sink()
        {

        }

        public static bool Debug
        {
            get
            {
                return s_Debug;
            }
        }

        private static void Configure(string filename)
        {
            SnCore.DomainMail.Configuration cnf = new SnCore.DomainMail.Configuration(filename);
            object debug = cnf["debug"];
            s_Debug = (debug == null) ? true : bool.Parse(debug.ToString());
            LogDebug(string.Format("Loaded configuration file \"{0}\".", filename));

            IDictionary hibernate = cnf.GetConfig("nhibernate");
            if (hibernate != null)
            {
                IDictionaryEnumerator enumerator = hibernate.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string name = enumerator.Key.ToString();
                    string value = enumerator.Value.ToString();
                    SnCore.Data.Hibernate.Session.Configuration.Properties[name] = value;
                    LogDebug(string.Format("{0}=\"{1}\".", name, value));
                }
            }
        }

        private static void LoadConfiguration()
        {
            string filename = Assembly.GetExecutingAssembly().Location + ".config";
            try
            {
                LogDebug(string.Format("Loading configuration file \"{0}\".", filename));
                if (File.Exists(filename))
                {
                    Configure(filename);
                }

                s_ConfigurationChangeWatcher = new FileSystemWatcher(Path.GetDirectoryName(filename), "*.config");
                s_ConfigurationChangeWatcher.Created += new FileSystemEventHandler(s_ConfigurationChangeWatcher_Changed);
                s_ConfigurationChangeWatcher.Changed += new FileSystemEventHandler(s_ConfigurationChangeWatcher_Changed);
            }
            catch (Exception ex)
            {
                LogError(string.Format("Error loading configuration file \"{0}\"\n{1}", filename, ex.Message));
            }
        }

        static void s_ConfigurationChangeWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Configure(e.FullPath);
        }

        private void UpdateFailureEmails(MimeDSNRecipient r, ISession session)
        {
            IList<AccountEmail> emails = session.CreateCriteria(typeof(AccountEmail))
                .Add(Expression.Eq("Address", r.FinalRecipientEmailAddress))
                .List<AccountEmail>();

            if (emails == null)
                return;

            foreach (AccountEmail email in emails)
            {
                if (email.Failed)
                    continue;

                Log(string.Format("Marked {0} [{1}] (id:{2}) with failure [{3}].",
                    email.Account.Name, email.Address, email.Id, r.DiagnosticCode));

                // check whether there're pending invitations for this e-mail
                foreach (AccountEmailConfirmation confirmation in Collection<AccountEmailConfirmation>.GetSafeCollection(email.AccountEmailConfirmations))
                {
                    session.Delete(confirmation);
                }

                email.Failed = true;
                email.LastError = r.DiagnosticCode;
                email.Modified = DateTime.UtcNow;
                session.Save(email);
                session.Flush();
            }
        }

        private void UpdateFailureInvitations(MimeDSNRecipient r, ISession session)
        {
            IList<AccountInvitation> invitations = session.CreateCriteria(typeof(AccountInvitation))
                .Add(Expression.Eq("Email", r.FinalRecipientEmailAddress))
                .List<AccountInvitation>();

            if (invitations == null)
                return;

            foreach (AccountInvitation invitation in invitations)
            {
                if (invitation.Failed)
                    continue;

                Log(string.Format("Marked \"{0}\" [invited by {1}] (id:{2}) with failure [{3}].",
                    invitation.Email, invitation.Account.Name, invitation.Id, r.DiagnosticCode));

                invitation.Failed = true;
                invitation.LastError = r.DiagnosticCode;
                invitation.Modified = DateTime.UtcNow;
                session.Save(invitation);
                session.Flush();
            }
        }

        private void UpdateFailure(MimeDSNRecipient r)
        {
            ISession session = SnCore.Data.Hibernate.Session.Factory.OpenSession();
            try
            {
                LogDebug(string.Format("Searching for {0}.", r.FinalRecipientEmailAddress));
                UpdateFailureEmails(r, session);
                UpdateFailureInvitations(r, session);
            }
            finally
            {
                session.Close();
            }
        }

        void IMailTransportSubmission.OnMessageSubmission(
             MailMsg mailmsg,
             IMailTransportNotify notify,
             IntPtr context)
        {
            LogDebug("Invoking mail sink message submission callback.");

            try
            {
                Message message = new Message(mailmsg);
                LogDebug(string.Format("Processing message \"{0}\".", message.Rfc822MsgSubject));
                string raw = Encoding.ASCII.GetString(message.ReadContent(0, message.GetContentSize()));

                MimeMessage msg = new MimeMessage();
                msg.LoadBody(raw);

                ArrayList bodylist = new ArrayList();
                msg.GetBodyPartList(bodylist);

                for (int i = 0; i < bodylist.Count; i++)
                {
                    MimeBody ab = (MimeBody)bodylist[i];
                    switch (ab.GetContentType())
                    {
                        case "message/delivery-status":
                            /// TODO: move to Mime processor
                            MimeDSN dsn = new MimeDSN();
                            dsn.LoadBody(ab.GetText());
                            foreach (MimeDSNRecipient r in dsn.Recipients)
                            {
                                switch (r.Action)
                                {
                                    case "failed":
                                        Log(string.Format("Processing {0} ({1}) in {2} with subject \"{3}\".",
                                            r.FinalRecipientEmailAddress, r.Action, message.Rfc822MsgId, message.Rfc822MsgSubject));
                                        UpdateFailure(r);
                                        break;
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message + "\n" + ex.StackTrace.ToString());
            }
            finally
            {
                if (null != mailmsg)
                    Marshal.ReleaseComObject(mailmsg);
            }
        }

        private static void Log(string message)
        {

            Log(message, EventLogEntryType.Information);
        }

        private static void Log(string message, EventLogEntryType type)
        {
            EventLogManager.WriteEntry(Assembly.GetExecutingAssembly().FullName,
              message, type);
        }

        private static void LogDebug(string message)
        {
            if (Debug)
            {
                Log(message);
            }
        }

        private static void LogError(string message)
        {
            Log(message, EventLogEntryType.Error);
        }
    }
}
