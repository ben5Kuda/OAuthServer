using IdSrvr4Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdSrvr4Demo.Repositories
{
  public interface IUserRepository
  {
    IEnumerable<User> GetAllUsers();
    User GetUserByKey(int key);
    User FindUserByUsernameOrEmail(string username);
    void Add(User user);
    void Save();
    void Update(User user);
    bool ValidateCredentials(string username, string password);
   
  }
}
