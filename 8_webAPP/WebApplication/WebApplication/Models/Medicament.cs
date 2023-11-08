using System.Collections.Generic;

namespace WebApplication.Models
{
    public class Medicament
    {
        // lek moze byc na wielu receptach
        public Medicament()
        {
            PrescriptionMedicaments = new HashSet<PrescriptionMedicament>();
        }
        
        public int IdMedicament { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        
        public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set;  }
    }
}