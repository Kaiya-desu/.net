using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication.DTO.Responses;
using WebApplication.Models;
using WebApplication.Repositories.Interfaces;

namespace WebApplication.Repositories.Implementations
{
    public class PrescriptionDbRepository: IPrescriptionDbRepository
    {
        private readonly HospitalContext _context;

        public PrescriptionDbRepository(HospitalContext context)
        {
            _context = context;
        }

        public async Task<PrescriptionResponseDTO> GetPrescription(int idPrescription)
        {
            var findPrescription = await _context.Prescription.AnyAsync(p => p.IdPrescription == idPrescription);
            if (!findPrescription)
                return null;

            // jak zrobic tablice w jednej metodzie?
            var medicaments = await _context.PrescriptionMedicament.Where(p => p.IdPrescription == idPrescription)
                .Include(n => n.IdMedicamentNavigatorion)
                .Select(pm => new MedicamentResponseDTO
                {
                    IdMedicament = pm.IdMedicament,
                    Name = pm.IdMedicamentNavigatorion.Name,
                    Description = pm.IdMedicamentNavigatorion.Description,
                    Type = pm.IdMedicamentNavigatorion.Type,
                    Dose = pm.Dose,
                    Details = pm.Details
                }).ToArrayAsync();

            var prescription = await _context.PrescriptionMedicament.Where(p => p.IdPrescription == idPrescription)
                .Include(n => n.IdPrescriptionNavigatorion)
                .Include(n => n.IdPrescriptionNavigatorion.IdDoctorNavigatorion)
                .Include(n => n.IdPrescriptionNavigatorion.IdPatientNavigatorion)
                .Select(p => new PrescriptionResponseDTO
                {
                   Date = p.IdPrescriptionNavigatorion.Date,
                   DueDate = p.IdPrescriptionNavigatorion.DueDate,
                   Patient = new PatientResponseDTO
                   {
                       IdPatient = p.IdPrescriptionNavigatorion.IdPatient,
                       FirstName = p.IdPrescriptionNavigatorion.IdPatientNavigatorion.FirstName,
                       LastName = p.IdPrescriptionNavigatorion.IdPatientNavigatorion.LastName,
                       Birthdate = p.IdPrescriptionNavigatorion.IdPatientNavigatorion.Birthdate
                   },
                   Doctor = new DoctorResponseDTO
                   {
                       IdDoctor = p.IdPrescriptionNavigatorion.IdDoctor,
                       FirstName = p.IdPrescriptionNavigatorion.IdDoctorNavigatorion.FirstName,
                       LastName = p.IdPrescriptionNavigatorion.IdDoctorNavigatorion.LastName,
                       Email = p.IdPrescriptionNavigatorion.IdDoctorNavigatorion.Email
                     },
                   Medicaments = medicaments
                }).FirstAsync();
            
            return prescription;
        }
    }
}