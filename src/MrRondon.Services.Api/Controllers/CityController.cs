using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using MrRondon.Domain.Entities;
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
                var cities = GetCities(_db).Where(x => x.Name.Contains(name));
                return Ok(cities);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("{subcategoryId:int}")]
        public IHttpActionResult Get(int subcategoryId)
        {
            try
            {
                var cities = GetCities(_db, subcategoryId);
                return Ok(cities);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public static IEnumerable<City> GetCities(MainContext db)
        {
            var cities = (from ci in db.Cities
                          join ad in db.Addresses on ci.CityId equals ad.CityId
                          join co in db.Companies on ad.AddressId equals co.AddressId
                          group ci by ci.Name
                into gp
                          select gp.Select(s => s)).SelectMany(s => s).AsNoTracking();

            return cities.Distinct();
        }

        public static IEnumerable<City> GetCities(MainContext db, int subCategoryId)
        {
            var cities = (from ci in db.Cities
                          join ad in db.Addresses on ci.CityId equals ad.CityId
                          join co in db.Companies on ad.AddressId equals co.AddressId
                          where co.SubCategoryId == subCategoryId
                          group ci by ci.Name
                into gp
                          select gp.Select(s => s)).SelectMany(s => s).AsNoTracking();

            return cities.Distinct();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}