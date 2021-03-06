using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountFeedItemTest : ManagedCRUDTest<AccountFeedItem, TransitAccountFeedItem, ManagedAccountFeedItem>
    {
        ManagedAccountFeedTest _accountfeed = new ManagedAccountFeedTest();

        public override void SetUp()
        {
            _accountfeed.SetUp();
            base.SetUp();
        }

        public override void TearDown()
        {
            base.TearDown();
            _accountfeed.TearDown();
        }

        public ManagedAccountFeedItemTest()
        {

        }

        public override TransitAccountFeedItem GetTransitInstance()
        {
            TransitAccountFeedItem t_instance = new TransitAccountFeedItem();
            t_instance.AccountFeedId = _accountfeed.Instance.Id;
            t_instance.AccountFeedLinkUrl = GetNewUri();
            t_instance.AccountFeedName = GetNewString();
            t_instance.Description = GetNewString();
            t_instance.Title = GetNewString();
            return t_instance;
        }
    }
}
