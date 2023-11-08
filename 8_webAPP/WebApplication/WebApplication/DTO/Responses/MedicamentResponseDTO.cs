namespace WebApplication.DTO.Responses
{
    public class MedicamentResponseDTO
    {
        public int IdMedicament { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        
        // dodatkowe informacje
        public int Dose { get; set; }
        public string Details { get; set; }
    }
}