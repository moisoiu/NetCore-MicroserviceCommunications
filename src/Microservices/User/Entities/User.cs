using System;
using System.Collections.Generic;

namespace User.Entities
{
    public partial class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public byte[] Salt { get; set; }
        public byte[] RowVersion { get; set; }
        public bool IsActive { get; set; }
    }
}
