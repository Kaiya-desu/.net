using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication.Models;

namespace WebApplication.Configurations
{
    public class PrescriptionMedicamentEfConfiguration : IEntityTypeConfiguration<PrescriptionMedicament>
    {
        public void Configure(EntityTypeBuilder<PrescriptionMedicament> builder)
            {
                // para kluczy obcych 
                builder.HasKey(pm => new { pm.IdMedicament, pm.IdPrescription}).HasName("Prescription_Medicament_pk");

                builder.ToTable("Prescription_Medicament");
                
                // klucz obcy robimy tak:
                builder.HasOne(pm => pm.IdMedicamentNavigatorion)
                    .WithMany(s => s.PrescriptionMedicaments)
                    .HasForeignKey(pm => pm.IdMedicament)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Medicament_Prescription_Medicament");

                builder.HasOne(pm => pm.IdPrescriptionNavigatorion)
                    .WithMany(p => p.PrescriptionMedicaments)
                    .HasForeignKey(pm => pm.IdPrescription)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Prescription_Prescription_Medicament");

                builder.Property(pm => pm.Dose);
                builder.Property(pm => pm.Details).HasMaxLength(100).IsRequired();
     
                
                var prescriptionsMedicaments = new List<PrescriptionMedicament>();
                prescriptionsMedicaments.Add(new PrescriptionMedicament{IdMedicament = 1, IdPrescription = 1, Dose = 1, Details = "Brać raz dziennie"});    
                prescriptionsMedicaments.Add(new PrescriptionMedicament{IdMedicament = 2, IdPrescription = 1, Dose = 2, Details = "Brać gdy boli głowa"});  
                
                prescriptionsMedicaments.Add(new PrescriptionMedicament{IdMedicament = 3, IdPrescription = 2, Dose = 1, Details = "Brać 3 razy dziennie"});    
                prescriptionsMedicaments.Add(new PrescriptionMedicament{IdMedicament = 2, IdPrescription = 2, Dose = 2, Details = "Brać przed snem"});  
                
                prescriptionsMedicaments.Add(new PrescriptionMedicament{IdMedicament = 3, IdPrescription = 3, Dose = 10, Details = "Używać gdy boli gardło"});    
                prescriptionsMedicaments.Add(new PrescriptionMedicament{IdMedicament = 4, IdPrescription = 3, Dose = 5, Details = "Używać gdy zatkany nos"}); 
                
                builder.HasData(prescriptionsMedicaments);
            }
    }
}