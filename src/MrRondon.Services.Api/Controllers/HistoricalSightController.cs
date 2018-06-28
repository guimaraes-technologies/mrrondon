using System.Data.Entity;
using System;
using System.Linq;
using System.Web.Http;
using MrRondon.Infra.Data.Context;
using WebApi.OutputCache.V2;

namespace MrRondon.Services.Api.Controllers
{
    [RoutePrefix("v1/historicalsight")]
    public class HistoricalSightController : ApiController
    {
        private readonly MainContext _db;

        public HistoricalSightController()
        {
            _db = new MainContext();
        }

        [AllowAnonymous]
        [Route("{id:int}")]
        [CacheOutput(ClientTimeSpan = 120, ServerTimeSpan = 120)]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var item = _db.HistoricalSights
                    .Include(i => i.Address.City)
                    .FirstOrDefault(f => f.HistoricalSightId == id);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("city/{cityId:int}/{name=}")]
        [CacheOutput(ClientTimeSpan = 120, ServerTimeSpan = 120, MustRevalidate = true)]
        public IHttpActionResult Get(int cityId, string name)
        {
            try
            {
                name = string.IsNullOrWhiteSpace(name) ? string.Empty : name;
                var items = _db.HistoricalSights
                    .Include(i => i.Address.City)
                    .Where(x => x.Address.CityId == cityId && x.Name.Contains(name));
                return Ok(items);
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