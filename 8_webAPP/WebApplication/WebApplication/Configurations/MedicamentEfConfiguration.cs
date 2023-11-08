using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication.Models;

namespace WebApplication.Configurations
{
    public class MedicamentEfConfiguration : IEntityTypeConfiguration<Medicament>
    {
        public void Configure(EntityTypeBuilder<Medicament> builder)
        {
            builder.HasKey(m => m.IdMedicament).HasName("Medicament_pk");

            builder.Property(m => m.IdMedicament).UseIdentityColumn();
            
            builder.Property(m => m.Name).HasMaxLength(100).IsRequired();
            builder.Property(m => m.Description).HasMaxLength(100).IsRequired();
            builder.Property(m => m.Type).HasMaxLength(100).IsRequired();

            var medicaments = new List<Medicament>();
            medicaments.Add(new Medicament{ IdMedicament = 1, Name = "Cerutin", Description = "Witamina C. Na przeziębienie", Type = "Tabletki"});
            medicaments.Add(new Medicament{ IdMedicament = 2, Name = "Apap", Description = "Przeciwbólowy. Zawiera paracetamol", Type = "Tabletki"});
            medicaments.Add(new Medicament{ IdMedicament = 3, Name = "Supremin", Description = "Przeciwzapalny, na kaszel", Type = "Syrop"});
            medicaments.Add(new Medicament{ IdMedicament = 4, Name = "Xylorin", Description = "NA katar", Type = "Krople"});
            
            builder.HasData(medicaments);
        }
    }
}