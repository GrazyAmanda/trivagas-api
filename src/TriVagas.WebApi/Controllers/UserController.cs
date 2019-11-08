using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TriVagas.Services.Interfaces;
using TriVagas.Services.Requests;

namespace TriVagas.Application.Controllers {
    [Produces ("application/json")]
    [Route ("user/")]
    [ApiController]
    public class UserController : ControllerBase {
        private IUserService _userService;
        private EmailService _emailService;
        //login e autenticacao
        [HttpPost]
        [Route ("login")]
        public IActionResult Login ([FromBody] User user) {
            var user = _userService.Authenticate (user.email, user.Password);

            if (user == null) {
                return Unauthorized (new { message = "Senha ou email incorretos ou Usuário sem cadastro !" });
            } else {
                return Ok (user);
            }

        }

        [HttpPost]
        [Route ("register")]
        public IActionResult Register ([FromBody] UserRegisterRequest user) {
            return Ok (user);
        }
        //verificar email e mandar codigo
        [HttpPost]
        [Route ("verifyemailaddress")]
        public IActionResult Register ([FromBody] UserRegisterRequest user) {
            var verfivy = _emailService.EnvioEmail (user.email);
            //salva o codigo do usuario
            user.Token = verfivy;
            _userService.Add (user);
            return Ok ();
        }
        //create user com verificacao do código
        [HttpPost]
        [Route ("createuser")]
        public IActionResult Register ([FromBody] UserRegisterRequest user, [FromBody] String codigo) {
            User usuario = from u in User
            where u.Token == codigo
            select u;
            //se exixtir  o codigo ele une o usuario 
            if (usuario != null) {
                usuario.setEmail (user.email);
                usuario.setUassword (user.password);
                _userService.Update (usuario);
                //faz a verificação com o token e retorna usuario e o token
                var user = _userService.Authenticate (user.email, user.Password);
                if (user == null) {
                    return Unauthorized (new { message = "Senha ou email incorretos ou Usuário sem cadastro !" });
                } else {
                    return Ok (user);
                }
            }

            return Ok (verfivy);
        }

    }
}