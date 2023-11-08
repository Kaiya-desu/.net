using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication.Models;

namespace WebApplication.Configurations
{
    public class AccountEfConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => new {a.IdAccount, a.Login}).HasName("Login_pk");

            builder.Property(a => a.IdAccount).UseIdentityColumn();
            
            builder.Property(a => a.Login).HasMaxLength(30).IsRequired();
            builder.Property(a => a.Password).IsRequired();
            builder.Property(a => a.Email).HasMaxLength(100).IsRequired();
            builder.Property(a => a.RefreshToken).HasColumnType("uniqueidentifier");
            builder.Property(a => a.TokenExpires);
            
            var accounts = new List<Account>();
           
            accounts.Add(new Account{IdAccount = 1, Login = "admin", 
                Password = new PasswordHasher<Account>().HashPassword(new Account(), "admin"), 
                Email = "admin@admin.pl"});
            
            builder.HasData(accounts);
        }
    }
}