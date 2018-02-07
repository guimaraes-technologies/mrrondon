using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MrRondon.Infra.CrossCutting.Helper
{
    public class EmailManager
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public ArrayList Recipients { get; set; }

        public string Sender => "naorespondi@gmail.com";

        public EmailManager(ArrayList recipients)
        {
            Recipients = recipients;
        }

        public async Task<bool> SendAsync()
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(Sender, "Mr Rondon Turismo"),
                    Priority = MailPriority.Normal,
                    IsBodyHtml = true,
                    Subject = Subject,
                    Body = Body,
                    SubjectEncoding = Encoding.GetEncoding("ISO-8859-1"),
                    BodyEncoding = Encoding.GetEncoding("ISO-8859-1")
                };

                if (HttpContext.Current.Request.IsLocal) mailMessage.To.Add(Sender);
                else foreach (var t in Recipients) mailMessage.To.Add(t.ToString());

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(Sender, "N4OR3sp0nd4"),
                    Port = 587
                };
                await smtp.SendMailAsync(mailMessage);
                return true;
            }
            catch { return false; }
        }

        public void ForgotPassword(string user, string sendTo, string url)
        {
            Subject = "Redefinir senha de acesso";
            Recipients = new ArrayList { sendTo.ToLower() };
            Body = $@"
                    <section style='width: 400px; padding: 1em;'>
                        <h1 style='text-align: center; text-transform: uppercase;margin: 0'>Mr Rondon<br /><small>Sistema Mr Rondon Turismo</small></h1>
                        <hr />
                        <p>Olá {user}<br /><br />Para redefinir sua senha no <a href='{url}'> Mr Rondon Turismo</a>, clique no link abaixo:</p>
                        <hr />
                        <a href='{url}' style='display: block;
                                           background: #337f3c;
                                           color: rgba(0, 0, 0, .6); 
                                           padding: 1em; 
                                           margin:1em 0;
                                           text-transform: uppercase;
                                           font-weight: 700; 
                                           text-align: center; 
                                           text-decoration: none;
                                           border-radius: .28571429rem; 
                                           box-shadow: 0 0 0 1px transparent inset, 0 0 0 0 rgba(34, 36, 38, .15) inset;
                                           -webkit-transition: opacity .1s ease, background-color .1s ease, color .1s ease, box-shadow .1s ease, background .1s ease;
                                           transition: opacity .1s ease, background-color .1s ease, color .1s ease, box-shadow .1s ease, background .1s ease; '>
                            Redefinir senha
                        </a>                        
                        <hr />
                        <h5 style='text-align: center; color: rgba(0, 0, 0, .5); text-transform: uppercase;margin: 0'>SWTUR - Superintendência de Turismo </h5>
                    </section>
                ";
        }
    }
}