using System;
using System.Linq;
using System.Web.Http;
using MrRondon.Infra.Data.Context;
using MrRondon.Services.Api.ViewModels;

namespace MrRondon.Services.Api.Controllers
{
    [RoutePrefix("v1/category")]
    public class CategoryController : ApiController
    {
        private readonly MainContext _db;

        public CategoryController()
        {
            _db = new MainContext();
        }

        [Route("{question}")]
        public IHttpActionResult Test(string question)
        {
            return Ok("Logou");
        }

        [AllowAnonymous]
        [Route("{name=}")]
        public IHttpActionResult Get(string name)
        {
            try
            {
                name = name ?? string.Empty;

                //var items = (from subCategory in _db.SubCategories
                //             join company in _db.Companies on subCategory.SubCategoryId equals company.SubCategoryId
                //             where subCategory.ShowOnApp
                //                   && subCategory.CategoryId == null
                //                   && subCategory.Name.Contains(name)
                //             select new CategoryListVm(subCategory, company.CompanyId != Guid.Empty)).ToList();

                var categories = _db.SubCategories
                    .Include("Companies")
                    .Where(x => x.ShowOnApp && x.CategoryId == null && x.Name.Contains(name)
                                )
                    .ToList();

                return Ok(categories);
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