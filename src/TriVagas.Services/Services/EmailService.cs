using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Entities;
using WebApi.Helpers;
using System.Net;
using System.Net.Mail;
using System.Net.Configuration;

namespace TriVagas.Services.Services
{

    public class EmailService 
    {
        public EmailService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public String EnvioEmail(string email)
        {  
         MailMessage mail = new MailMessage();
            String codigo = (new Date().getTime()+"").substring(2, 8);

            mail.From = new MailAddress("teste@gmail.com");
            mail.To.Add(email); // para
            mail.Subject = "Email de validacao"; // assunto
            mail.Body = "Seu código é " + codigo + " digite ele para autenticar seu login ! "; // mensagem

            //config o meu gmail
            var smtp = new SmtpClient("smtp.gmail.com"))

            smtp.EnableSsl = true; // GMail requer SSL
            smtp.Port = 587;       // porta para SSL
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // modo de envio
            smtp.UseDefaultCredentials = false; // vamos utilizar credencias especificas

            // seu usuário e senha para autenticação
            smtp.Credentials = new NetworkCredential("teste@gmail.com", "***");

            // envia o e-mail
            smtp.Send(mail);
            return codigo;

        }
            
        
    }
}