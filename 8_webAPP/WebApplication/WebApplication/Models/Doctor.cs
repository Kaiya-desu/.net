using System.Collections.Generic;

namespace WebApplication.Models
{
    public class Doctor
    {
        // lekarz moze wypisac wiele recept
        public Doctor()
        {
            Prescriptions = new HashSet<Prescription>();
        }
        
        public int IdDoctor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
        public virtual ICollection<Prescription> Prescriptions { get; set;  }
        
    }
}