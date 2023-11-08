using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication.Models;

namespace WebApplication.Configurations
{
    public class PatientEfConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(p => p.IdPatient).HasName("Patient_pk");

            builder.Property(p => p.IdPatient).UseIdentityColumn();
            
            builder.Property(p => p.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(p => p.LastName).HasMaxLength(100).IsRequired();
            
            var patients = new List<Patient>();
            patients.Add(new Patient{IdPatient = 1, FirstName = "Maciek", LastName = "Woźniak", Birthdate = new DateTime(2000,12,11)});
            patients.Add(new Patient{IdPatient = 2, FirstName = "Asia", LastName = "Pietrzak", Birthdate = new DateTime(1997,1,31)});
            
            builder.HasData(patients);
        }
    }
}