using System;

namespace WebApplication.DTO.Responses
{
    public class PrescriptionResponseDTO
    {
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public PatientResponseDTO Patient { get; set; }
        public DoctorResponseDTO Doctor { get; set; }
        public MedicamentResponseDTO[] Medicaments { get; set; }
    }
}