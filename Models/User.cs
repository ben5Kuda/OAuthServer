using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdSrvr4Demo.Models
{
  public class User
  {
    public int UsersId { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Profile { get; set; }
    public bool IsLockedOut { get; set; }
    public int PasswordRetryCount { get; set; }
    public string Email { get; set; }
    public string Provider { get; set; }
    public string UserProviderId { get; set; }
  }
}
