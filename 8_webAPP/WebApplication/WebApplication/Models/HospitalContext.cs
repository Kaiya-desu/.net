using Microsoft.EntityFrameworkCore;
using WebApplication.Configurations;

namespace WebApplication.Models
{
    public class HospitalContext : DbContext
    {
        public HospitalContext()
        {
            
        }

        public HospitalContext(DbContextOptions<HospitalContext> options) : base(options)
        {
            
        }
        
        public virtual DbSet<Doctor> Doctor{ get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<Prescription> Prescription { get; set; }
        public virtual DbSet<PrescriptionMedicament> PrescriptionMedicament { get; set; }
        public virtual DbSet<Medicament> Medicament{ get; set; }
        
        public virtual DbSet<Account> Account{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DoctorEfConfiguration());
            modelBuilder.ApplyConfiguration(new PatientEfConfiguration());
            modelBuilder.ApplyConfiguration(new PrescriptionEfConfiguration());
            modelBuilder.ApplyConfiguration(new MedicamentEfConfiguration());
            modelBuilder.ApplyConfiguration(new PrescriptionMedicamentEfConfiguration());
            
            modelBuilder.ApplyConfiguration(new AccountEfConfiguration());
        }

    }
}