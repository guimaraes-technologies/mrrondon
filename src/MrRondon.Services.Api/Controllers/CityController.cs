using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using MrRondon.Infra.Data.Context;

namespace MrRondon.Services.Api.Controllers
{
    [RoutePrefix("v1/city")]
    public class CityController : ApiController
    {
        private readonly MainContext _db;

        public CityController()
        {
            _db = new MainContext();
        }

        [AllowAnonymous]
        [Route("{name=}")]
        public IHttpActionResult Get(string name)
        {
            try
            {
                name = name ?? string.Empty;

                var cities = (from ci in _db.Cities
                              join ad in _db.Addresses on ci.CityId equals ad.CityId
                              where ci.Name.Contains(name)
                              group ci by ci.Name
                    into gp
                              select gp.Select(s => s)).SelectMany(s => s).AsNoTracking();

                var result = cities.Distinct();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("first/{name}")]
        public IHttpActionResult GetFirst(string name)
        {
            try
            {
                name = name ?? string.Empty;

                var cities = (from ci in _db.Cities
                    join ad in _db.Addresses on ci.CityId equals ad.CityId
                    where ci.Name.Contains(name)
                    group ci by ci.Name
                    into gp
                    select gp.Select(s => s)).SelectMany(s => s).AsNoTracking();

                var result = cities.Distinct();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("has/company/subcategory/{subCategoryId:int}")]
        public IHttpActionResult GetWithCompany(int subCategoryId)
        {
            try
            {
                var cities = (from ci in _db.Cities
                              join ad in _db.Addresses on ci.CityId equals ad.CityId
                              join co in _db.Companies on ad.AddressId equals co.AddressId
                              where co.SubCategoryId == subCategoryId
                              group ci by ci.Name
                into gp
                              select gp.Select(s => s)).SelectMany(s => s).AsNoTracking();

                var result = cities.Distinct();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("has/event")]
        public IHttpActionResult GetWithEvent()
        {
            try
            {
                var cities = (from ci in _db.Cities
                    join ad in _db.Addresses on ci.CityId equals ad.CityId
                    join ev in _db.Events on ad.AddressId equals ev.AddressId
                    group ci by ci.Name
                    into gp
                    select gp.Select(s => s)).SelectMany(s => s).AsNoTracking();

                var result = cities.Distinct();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("has/historicalsight")]
        public IHttpActionResult GetWithHistoricalSightAsync()
        {
            try
            {
                var cities = (from ci in _db.Cities
                    join ad in _db.Addresses on ci.CityId equals ad.CityId
                    join h in _db.HistoricalSights on ad.AddressId equals h.AddressId
                    group ci by ci.Name
                    into gp
                    select gp.Select(s => s)).SelectMany(s => s).AsNoTracking();

                var result = cities.Distinct();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}