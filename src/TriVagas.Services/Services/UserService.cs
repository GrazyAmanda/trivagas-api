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

namespace TriVagas.Services.Services
{

    public class UserService : IUserService
    {
       
        private List<User> _users = new List<User>
        { 
            new User { ProfileId = 1, Email = "test@gmail.com", Profile = "dev", Password = "test" } 
        };

        private readonly AppSettings _appSettings;
         private readonly IUserService _users;
        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public User Authenticate(string email, string password)
        {
            var user = _users.SingleOrDefault(x => x.Email == email && x.Password == password);
             // Retorna vazio se não for encontrado
            if (user == null)
                return null;
            //se achar retorna o JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                //7 dia de expriração
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

           //remove a senha 
            user.Password = null;

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            // retorna os usuarios sem senha
            return _users.Select(x => {
                x.Password = null;
                return x;
            });
        }
        public void Update(User obj)
        {
            _users.Update(obj);
        }
        public void Add(User obj)
        {
            _users.Add(obj);
        }
    }
}