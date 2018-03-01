using System.Data.Entity;
using System;
using System.Linq;
using System.Web.Http;
using MrRondon.Infra.Data.Context;

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