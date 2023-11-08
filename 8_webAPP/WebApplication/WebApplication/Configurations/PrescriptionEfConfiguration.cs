using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication.Models;

namespace WebApplication.Configurations
{
    public class PrescriptionEfConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.HasKey(p => p.IdPrescription).HasName("Prescription_pk");

            builder.Property(p => p.IdPrescription).UseIdentityColumn();
            
            builder.Property(p => p.Date).HasColumnType("datetime").IsRequired();
            builder.Property(p => p.DueDate).HasColumnType("datetime").IsRequired();
            
            // klucz obcy robimy tak:
            builder.HasOne(p => p.IdPatientNavigatorion)
                .WithMany(s => s.Prescriptions)
                .HasForeignKey(p => p.IdPatient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Patient_Prescription");

            builder.HasOne(p => p.IdDoctorNavigatorion)
                .WithMany(d => d.Prescriptions)
                .HasForeignKey(p => p.IdDoctor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Doctor_Prescription");


            var prescriptions = new List<Prescription>();
            // albo Date = DateTime.Now, DueDate = DateTime.Now.AddMonths(1), 
            prescriptions.Add(new Prescription{IdPrescription = 1, Date = new DateTime(2020,4,20), DueDate = new DateTime(2020,5,20), IdPatient = 1, IdDoctor = 1});
            prescriptions.Add(new Prescription{IdPrescription = 2, Date = new DateTime(2021,3,22), DueDate = new DateTime(2021,4,22), IdPatient = 1, IdDoctor = 2});
            prescriptions.Add(new Prescription{IdPrescription = 3, Date = new DateTime(2021,4,1), DueDate = new DateTime(2021,5,1), IdPatient = 2, IdDoctor = 1});
            
            builder.HasData(prescriptions);
        }
    }
}