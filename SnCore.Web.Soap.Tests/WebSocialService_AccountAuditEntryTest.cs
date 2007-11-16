using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebSocialServiceTests
{
    [TestFixture]
    public class AccountAuditEntryTest : WebServiceBaseTest<WebSocialServiceNoCache>
    {

        public AccountAuditEntryTest()
        {

        }

        [Test]
        public void GetAccountFriendAuditEntriesTest()
        {
            // make a new user, add admin to his friends
            string email = GetNewEmailAddress();
            string password = "password";
            int user_id = CreateUser(email, password);
            string ticket = Login(email, password);
            // admin makes a friends request
            int friend_request_id = EndPoint.CreateOrUpdateAccountFriendRequest(GetAdminTicket(), user_id, GetNewString());
            Console.WriteLine("Created friend request: {0}", friend_request_id);
            EndPoint.AcceptAccountFriendRequest(ticket, friend_request_id, GetNewString());
            // $(TODO): the user now has an audit entry that he has a new friend
            int audits_count = EndPoint.GetAccountFriendAuditEntriesCount(GetAdminTicket(), GetAdminAccount().Id);
            Console.WriteLine("Audit entries: {0}", audits_count);
            // delete the user
            DeleteUser(user_id); 
        }
    }
}
