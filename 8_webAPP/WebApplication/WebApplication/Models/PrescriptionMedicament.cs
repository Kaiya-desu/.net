namespace WebApplication.Models
{
    public class PrescriptionMedicament
    {
        public int IdMedicament { get; set; }
        public int IdPrescription { get; set; }
        public int Dose { get; set; }
        public string Details { get; set; }
        
        public virtual Medicament IdMedicamentNavigatorion { get; set; }
        public virtual Prescription IdPrescriptionNavigatorion { get; set; }
    }
}