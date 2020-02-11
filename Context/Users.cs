using System;
using System.Collections.Generic;

namespace IdSrvr4Demo.Context
{
    public partial class Users
    {
        public string UsersId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Profile { get; set; }
        public bool IsLockedOut { get; set; }
        public int PasswordRetryCount { get; set; }
        public string Password { get; set; }
        public int? TeamId { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
