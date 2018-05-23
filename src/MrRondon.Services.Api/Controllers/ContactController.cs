using System;
using System.Collections;
using System.Threading.Tasks;
using System.Web.Http;
using MrRondon.Infra.CrossCutting.Helper;
using MrRondon.Services.Api.ViewModels;

namespace MrRondon.Services.Api.Controllers
{
    [RoutePrefix("v1/contact")]
    public class ContactController : ApiController
    {
        [Route("send")]
        [HttpPost, AllowAnonymous]
        public async Task<IHttpActionResult> Send([FromBody]ContactMessage contactMessage)
        {
            try
            {
                var emailManager = new EmailManager(new ArrayList { contactMessage.Email });
                emailManager.NewContact(contactMessage.Subject, contactMessage.Email, contactMessage.Name, contactMessage.Cellphone, contactMessage.Telephone, contactMessage.Message);
                await emailManager.SendAsync(contactMessage.Email, $"Aplicativo {Constants.SystemName}");

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}