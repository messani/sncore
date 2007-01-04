using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountFriendRequestTest : ManagedCRUDTest<AccountFriendRequest, TransitAccountFriendRequest, ManagedAccountFriendRequest>
    {
        private ManagedAccountTest _account1 = new ManagedAccountTest();
        private ManagedAccountTest _account2 = new ManagedAccountTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _account1.SetUp();
            _account2.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _account2.TearDown();
            _account1.TearDown();
            base.TearDown();
        }

        public ManagedAccountFriendRequestTest()
        {

        }

        public override TransitAccountFriendRequest GetTransitInstance()
        {
            TransitAccountFriendRequest t_instance = new TransitAccountFriendRequest();
            t_instance.AccountId = _account1.Instance.Id;
            t_instance.KeenId = _account2.Instance.Id;
            t_instance.Message = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
