using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebLocationServiceTests
{
    [TestFixture]
    public class StateTest : WebServiceTest<WebLocationService.TransitState>
    {
        public CountryTest _country = new CountryTest();
        public int _country_id = 0;

        [SetUp]
        public void SetUp()
        {
            _country_id = _country.Create(GetAdminTicket());
        }

        [TearDown]
        public void TearDown()
        {
            _country.Delete(GetAdminTicket(), _country_id);
        }

        public StateTest()
            : base("State", new WebLocationServiceNoCache())
        {
        }


        public override WebLocationService.TransitState GetTransitInstance()
        {
            WebLocationService.TransitState t_instance = new WebLocationService.TransitState();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Country = (string) _country.GetInstancePropertyById(GetAdminTicket(), _country_id, "Name");
            return t_instance;
        }

        [Test]
        public void GetStatesByCountryIdTest()
        {
            WebLocationService.TransitState t_instance = GetTransitInstance();
            WebLocationServiceNoCache endpoint = (WebLocationServiceNoCache) EndPoint;
            int id = endpoint.CreateOrUpdateState(GetAdminTicket(), t_instance);
            int count = endpoint.GetStatesByCountryIdCount(GetAdminTicket(), _country_id);
            Assert.IsTrue(count > 0);
            WebLocationService.TransitState[] states = endpoint.GetStatesByCountryId(GetAdminTicket(), _country_id, null);
            bool bFound = false;
            foreach (WebLocationService.TransitState state in states)
            {
                if (state.Id == id)
                {
                    bFound = true;
                    break;
                }
            }
            Assert.IsTrue(bFound, "State was not returned from GetStatesByCountryId");
            endpoint.DeleteState(GetAdminTicket(), id);
        }
    }
}