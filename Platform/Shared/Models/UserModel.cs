using System;
using System.Collections.Generic;

namespace Platform.Shared.Models
{
    public class UserModel
    {

        public UserModel()
        {
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }

        public IList<string> Roles { get; set; }
    }
}
