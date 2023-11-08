using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication.Models;

namespace WebApplication.Configurations
{
    public class DoctorEfConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(d => d.IdDoctor).HasName("Doctor_pk");

            builder.Property(d => d.IdDoctor).UseIdentityColumn();
            
            builder.Property(d => d.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(d => d.LastName).HasMaxLength(100).IsRequired();
            builder.Property(d => d.Email).HasMaxLength(100).IsRequired();

            var doctors = new List<Doctor>();
            doctors.Add(new Doctor{IdDoctor = 1, FirstName = "Adam", LastName = "Kowalski", Email = "a.kowalski@poczta.pl"});
            doctors.Add(new Doctor{IdDoctor = 2, FirstName = "Monika", LastName = "Nowak", Email = "m.nowak@poczta.pl"});

            builder.HasData(doctors);
        }
    }
}