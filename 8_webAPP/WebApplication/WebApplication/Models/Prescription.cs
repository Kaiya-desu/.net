using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public class Prescription
    {
        // bo jest tabela asocjacyjna - wiele do wielu
        public Prescription()
        {
            PrescriptionMedicaments = new HashSet<PrescriptionMedicament>();
        }
        
        public int IdPrescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public int IdPatient { get; set; }
        public int IdDoctor { get; set; }
        
        // bo posiada klucze obce idDoktora i idPacjenta 
        public virtual Doctor IdDoctorNavigatorion { get; set; }
        public virtual Patient IdPatientNavigatorion { get; set; }
        
        // bo jest kluczem obcym w prescription_Medicament
        public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set;  }
    }
}