using System;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace WebApplication.Models
{
    public class Account
    {
        public int IdAccount { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Guid RefreshToken { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}