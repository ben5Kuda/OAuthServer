using IdSrvr4Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdSrvr4Demo.Repositories
{
  public class UserRepository : IUserRepository
  {
    public void Add(User user)
    {
      // this would typically add a user
    }

    public IEnumerable<User> GetAllUsers()
    {
      return Query();
    }

    public User GetUserByKey(int key)
    {
      return Query().Where(x => x.UsersId == key).FirstOrDefault();
    }

    public User FindUserByUsernameOrEmail(string username)
    {
      return Query().Where(x => x.Username == username).FirstOrDefault();
    }

    public void Save()
    {
      // in a real life scenario this will persist the changes
    }

    public void Update(User user)
    {
      // this would typically update the user
    }

    public bool ValidateCredentials(string username, string password)
    {
      return Query().Where(x => x.Username == username)
             .Where(p => p.Password == password).Any();
    }

    private IEnumerable<User> Query()
    {
      return new List<User>
      {
        new User
        {
          UsersId = 10,
          Name = "Kuda",
          Profile = "Everyone",
          IsLockedOut = false,
          Password = "p@ssword",
          PasswordRetryCount = 0,
          Username = "kuda@github.com",
          Email = "kuda@github.com"
        }
      };
    }
   
  }
}
