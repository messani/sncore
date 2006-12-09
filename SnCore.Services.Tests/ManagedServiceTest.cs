using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using NHibernate.Expression;
using System.Diagnostics;
using SnCore.Tools.Web;

namespace SnCore.Services.Tests
{
    public class ManagedServiceTest : NHibernateTest
    {
        private bool mEnableRemoteContent = false;

        public override void SetUp()
        {
            mEnableRemoteContent = ContentPage.EnableRemoteContent;
            ContentPage.EnableRemoteContent = false;
            base.SetUp();
        }

        public override void TearDown()
        {
            base.TearDown();
            ContentPage.EnableRemoteContent = mEnableRemoteContent;
        }
    }
}