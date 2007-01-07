using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using SnCore.Services;
using System.Diagnostics;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Expression;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;

namespace SnCore.WebServices
{

    /// <summary>
    /// The WebLocationService provides location information.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebLocationService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebLocationService : WebService
    {

        public WebLocationService()
        {

        }

        #region Country

        /// <summary>
        /// Create or update a country.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="country">transit country</param>
        [WebMethod(Description = "Create or update a country.")]
        public int CreateOrUpdateCountry(string ticket, TransitCountry country)
        {
            return WebServiceImpl<TransitCountry, ManagedCountry, Country>.CreateOrUpdate(
                ticket, country);
        }

        /// <summary>
        /// Get a country.
        /// </summary>
        /// <returns>transit country</returns>
        [WebMethod(Description = "Get a country.")]
        public TransitCountry GetCountryById(string ticket, int id)
        {
            return WebServiceImpl<TransitCountry, ManagedCountry, Country>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all countries.
        /// </summary>
        /// <returns>list of transit countries</returns>
        [WebMethod(Description = "Get all countries.")]
        public List<TransitCountry> GetCountries(string ticket, ServiceQueryOptions options)
        {
            Order[] orders = { Order.Asc("Name") }; 
            return WebServiceImpl<TransitCountry, ManagedCountry, Country>.GetList(
                ticket, options, null, orders);

            //TODO: reimplement in UI
            //if (options == null)
            //{
            //    foreach (TransitCountry country in result)
            //    {
            //        if (country.Name == "United States")
            //        {
            //            result.Insert(0, new TransitCountry());
            //            result.Insert(0, country);
            //            break;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Get all countries count.
        /// </summary>
        /// <returns>number of countries</returns>
        [WebMethod(Description = "Get all countries count.")]
        public int GetCountriesCount(string ticket)
        {
            return WebServiceImpl<TransitCountry, ManagedCountry, Country>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete a country.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a country.")]
        public void DeleteCountry(string ticket, int id)
        {
            WebServiceImpl<TransitCountry, ManagedCountry, Country>.Delete(
                ticket, id);
        }

        #endregion

        #region State

        /// <summary>
        /// Create or update a state.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="state">transit state</param>
        [WebMethod(Description = "Create or update a state.")]
        public int CreateOrUpdateState(string ticket, TransitState state)
        {
            return WebServiceImpl<TransitState, ManagedState, State>.CreateOrUpdate(
                ticket, state);
        }

        /// <summary>
        /// Get a state.
        /// </summary>
        /// <returns>transit state</returns>
        [WebMethod(Description = "Get a state.")]
        public TransitState GetStateById(string ticket, int id)
        {
            return WebServiceImpl<TransitState, ManagedState, State>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all states.
        /// </summary>
        /// <returns>list of transit states</returns>
        [WebMethod(Description = "Get all states.")]
        public List<TransitState> GetStates(string ticket, ServiceQueryOptions options)
        {
            Order[] orders = { Order.Asc("Name") };
            return WebServiceImpl<TransitState, ManagedState, State>.GetList(
                ticket, options, null, orders);
        }

        /// <summary>
        /// Get all states count.
        /// </summary>
        /// <returns>number of states</returns>
        [WebMethod(Description = "Get all states count.")]
        public int GetStatesCount(string ticket)
        {
            return WebServiceImpl<TransitState, ManagedState, State>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get all states within a country.
        /// </summary>
        /// <returns>list of transit states</returns>
        [WebMethod(Description = "Get all states.")]
        public List<TransitState> GetStatesByCountryId(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Country.Id", id) };
            Order[] orders = { Order.Asc("Name") };
            return WebServiceImpl<TransitState, ManagedState, State>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Get all states within a country count.
        /// </summary>
        /// <returns>number of states</returns>
        [WebMethod(Description = "Get all states within a country count.")]
        public int GetStatesByCountryIdCount(string ticket, int id)
        {
            return WebServiceImpl<TransitState, ManagedState, State>.GetCount(
                ticket, string.Format("WHERE State.Country.Id = {0}", id));
        }

        /// <summary>
        /// Delete a state.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a state.")]
        public void DeleteState(string ticket, int id)
        {
            WebServiceImpl<TransitState, ManagedState, State>.Delete(
                ticket, id);
        }

        #endregion

        #region City

        /// <summary>
        /// Create or update a city.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="city">transit city</param>
        [WebMethod(Description = "Create or update a city.")]
        public int CreateOrUpdateCity(string ticket, TransitCity city)
        {
            return WebServiceImpl<TransitCity, ManagedCity, City>.CreateOrUpdate(
                ticket, city);
        }

        /// <summary>
        /// Get a city.
        /// </summary>
        /// <returns>transit city</returns>
        [WebMethod(Description = "Get a city.")]
        public TransitCity GetCityById(string ticket, int id)
        {
            return WebServiceImpl<TransitCity, ManagedCity, City>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all cities.
        /// </summary>
        /// <returns>list of transit cities</returns>
        [WebMethod(Description = "Get all cities.")]
        public List<TransitCity> GetCities(string ticket, ServiceQueryOptions options)
        {
            Order[] orders = { Order.Asc("Name") };
            return WebServiceImpl<TransitCity, ManagedCity, City>.GetList(
                ticket, options, null, orders);
        }

        /// <summary>
        /// Get all cities count.
        /// </summary>
        /// <returns>number of cities</returns>
        [WebMethod(Description = "Get all cities count.")]
        public int GetCitiesCount(string ticket)
        {
            return WebServiceImpl<TransitCity, ManagedCity, City>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get all cities within a country.
        /// </summary>
        /// <returns>list of transit cities</returns>
        [WebMethod(Description = "Get all cities within a country.")]
        public List<TransitCity> GetCitiesByCountryId(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Country.Id", id) };
            Order[] orders = { Order.Asc("Name") };
            return WebServiceImpl<TransitCity, ManagedCity, City>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Get all cities within a state.
        /// </summary>
        /// <returns>list of transit cities</returns>
        [WebMethod(Description = "Get all cities within a state.")]
        public List<TransitCity> GetCitiesByStateId(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("State.Id", id) };
            Order[] orders = { Order.Asc("Name") };
            return WebServiceImpl<TransitCity, ManagedCity, City>.GetList(
                ticket, options, expressions, orders);
        }


        /// <summary>
        /// Get all cities within a country count.
        /// </summary>
        /// <returns>number of cities</returns>
        [WebMethod(Description = "Get all cities within a country count.")]
        public int GetCitiesByCountryIdCount(string ticket, int id)
        {
            return WebServiceImpl<TransitCity, ManagedCity, City>.GetCount(
                ticket, string.Format("WHERE City.Country.Id = {0}", id));
        }

        /// <summary>
        /// Get all cities within a state count.
        /// </summary>
        /// <returns>number of cities</returns>
        [WebMethod(Description = "Get all cities within a state count.")]
        public int GetCitiesByStateIdCount(string ticket, int id)
        {
            return WebServiceImpl<TransitCity, ManagedCity, City>.GetCount(
                ticket, string.Format("WHERE City.State.Id = {0}", id));
        }

        /// <summary>
        /// Delete a city.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a city.")]
        public void DeleteCity(string ticket, int id)
        {
            WebServiceImpl<TransitCity, ManagedCity, City>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get all cities with a matching name.
        /// </summary>
        /// <returns>list of cities</returns>
        [WebMethod(Description = "Get all cities with a matching name.", CacheDuration = 60)]
        public List<TransitCity> SearchCitiesByName(string ticket, string name, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Like("Name", string.Format("%{0}%", name)) };
            return WebServiceImpl<TransitCity, ManagedCity, City>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get city by tag.
        /// </summary>
        /// <param name="tag">city tag</param>
        /// <returns></returns>
        [WebMethod(Description = "Get city by tag.")]
        public TransitCity GetCityByTag(string ticket, string tag)
        {
            try
            {
                return WebServiceImpl<TransitCity, ManagedCity, City>.GetByCriterion(
                    ticket, Expression.Eq("Tag", tag));
            }
            catch (ObjectNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Merge cities.
        /// <param name="ticket">authentication ticket</param>
        /// </summary>
        [WebMethod(Description = "Merge cities.")]
        public int MergeCities(string ticket, int target_id, int merge_id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedCity m = new ManagedCity(session, target_id);
                int result = m.Merge(sec, merge_id);
                session.Flush();
                return result;
            }
        }

        #endregion

        #region Neighborhood

        /// <summary>
        /// Create or update a neighborhood.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="neighborhood">transit neighborhood</param>
        [WebMethod(Description = "Create or update a neighborhood.")]
        public int CreateOrUpdateNeighborhood(string ticket, TransitNeighborhood neighborhood)
        {
            return WebServiceImpl<TransitNeighborhood, ManagedNeighborhood, Neighborhood>.CreateOrUpdate(
                ticket, neighborhood);
        }

        /// <summary>
        /// Get a neighborhood.
        /// </summary>
        /// <returns>transit neighborhood</returns>
        [WebMethod(Description = "Get a neighborhood.")]
        public TransitNeighborhood GetNeighborhoodById(string ticket, int id)
        {
            return WebServiceImpl<TransitNeighborhood, ManagedNeighborhood, Neighborhood>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all neighborhoods.
        /// </summary>
        /// <returns>list of transit neighborhoods</returns>
        [WebMethod(Description = "Get all neighborhoods.")]
        public List<TransitNeighborhood> GetNeighborhoods(string ticket, ServiceQueryOptions options)
        {
            Order[] orders = { Order.Asc("Name") };
            return WebServiceImpl<TransitNeighborhood, ManagedNeighborhood, Neighborhood>.GetList(
                ticket, options, null, orders);
        }

        /// <summary>
        /// Get all neighborhoods count.
        /// </summary>
        /// <returns>number of neighborhoods</returns>
        [WebMethod(Description = "Get all neighborhoods count.")]
        public int GetNeighborhoodsCount(string ticket)
        {
            return WebServiceImpl<TransitNeighborhood, ManagedNeighborhood, Neighborhood>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get all neighborhoods within a city.
        /// </summary>
        /// <returns>list of transit neighborhoods</returns>
        [WebMethod(Description = "Get all neighborhoods within a city.")]
        public List<TransitNeighborhood> GetNeighborhoodsByCityId(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("City.Id", id) };
            Order[] orders = { Order.Asc("Name") };
            return WebServiceImpl<TransitNeighborhood, ManagedNeighborhood, Neighborhood>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Get all neighborhouds within a city count.
        /// </summary>
        /// <returns>number of states</returns>
        [WebMethod(Description = "Get all neighborhoods within a city count.")]
        public int GetNeighborhoodsByCityIdCount(string ticket, int id)
        {
            return WebServiceImpl<TransitNeighborhood, ManagedNeighborhood, Neighborhood>.GetCount(
                ticket, string.Format("WHERE Neighborhood.City.Id = {0}", id));
        }

        /// <summary>
        /// Delete a neighborhood.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a neighborhood.")]
        public void DeleteNeighborhood(string ticket, int id)
        {
            WebServiceImpl<TransitNeighborhood, ManagedNeighborhood, Neighborhood>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get all neighborhoods with a matching name.
        /// </summary>
        /// <returns>list of neighborhoods</returns>
        [WebMethod(Description = "Get all neighborhoods with a matching name.", CacheDuration = 60)]
        public List<TransitNeighborhood> SearchNeighborhoodsByName(string ticket, string name, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Like("Name", string.Format("%{0}%", name)) };
            return WebServiceImpl<TransitNeighborhood, ManagedNeighborhood, Neighborhood>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Merge neighborhoods.
        /// <param name="ticket">authentication ticket</param>
        /// </summary>
        [WebMethod(Description = "Merge neighborhoods.")]
        public int MergeNeighborhoods(string ticket, int target_id, int merge_id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedNeighborhood m = new ManagedNeighborhood(session, target_id);
                int result = m.Merge(sec, merge_id);
                session.Flush();
                return result;
            }
        }

        #endregion
    }
}