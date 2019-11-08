using System;
using System.Collections.Generic;
using TriVagas.Domain.Models;

namespace TriVagas.Services.Interfaces
{
   public interface IUserService : IRepository<User>
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        
    }
}



