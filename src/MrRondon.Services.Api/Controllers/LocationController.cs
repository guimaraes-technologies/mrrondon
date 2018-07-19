using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using MrRondon.Infra.Data.Context;
using MrRondon.Services.Api.Helpers;
using MrRondon.Services.Api.ViewModels;
using WebApi.OutputCache.V2;

namespace MrRondon.Services.Api.Controllers
{
    [RoutePrefix("v1/location")]
    public class LocationController : ApiController
    {
        private readonly MainContext _db;

        public LocationController()
        {
            _db = new MainContext();
        }

        [AllowAnonymous]
        [Route("nearby/meters/{precision:int}/latitude/{latitudeFrom}/longitude/{longitudeFrom}")]
        [CacheOutput(ClientTimeSpan = 120, ServerTimeSpan = 240, MustRevalidate = true)]
        public IHttpActionResult GetNearby(int precision, string latitudeFrom, string longitudeFrom)
        {
            try
            {
                var latitude = double.Parse(latitudeFrom);
                var longitude = double.Parse(longitudeFrom);
                var events = _db.Events
                    .Include(i => i.Address.City)
                    .Where(x => x.StartDate >= DateTime.Today)
                    .AsNoTracking().ToList();
                var eventsNearby = (from item in events
                                   where GeoLocatorHelper.PlacesAround(latitude, longitude, item.Address.Latitude, item.Address.Longitude, precision) <= precision
                                   select item).ToList();

                var items = (from item in eventsNearby let position = new Position(item.Address.Latitude, item.Address.Longitude) select new LocationVm(item.EventId.ToString(), item.Name, item.Address.FullAddress, PinType.Place, LocationType.Event, position, item.Logo)).ToList();

                var historicalSights = _db.HistoricalSights.Include(i => i.Address.City).AsNoTracking().ToList();
                var historicalSightsNearby = (from item in historicalSights
                                          where GeoLocatorHelper.PlacesAround(latitude, longitude, item.Address.Latitude, item.Address.Longitude, precision) <= precision
                    select item).ToList();

                items.AddRange((from item in historicalSightsNearby let position = new Position(item.Address.Latitude, item.Address.Longitude) select new LocationVm(item.HistoricalSightId.ToString(), item.Name, item.Address.FullAddress, PinType.Place, LocationType.Event, position, item.Logo)));

                var companies = _db.Companies.Include(i => i.Address.City).AsNoTracking().ToList();
                var companiesNearby = (from item in companies
                                       where GeoLocatorHelper.PlacesAround(latitude, longitude, item.Address.Latitude, item.Address.Longitude, precision) <= precision
                    select item).ToList();

                items.AddRange((from item in companiesNearby let position = new Position(item.Address.Latitude, item.Address.Longitude) select new LocationVm(item.CompanyId.ToString(), item.Name, item.Address.FullAddress, PinType.Place, LocationType.Event, position, item.Logo)));

                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}