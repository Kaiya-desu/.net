using System;
using System.Collections.Generic;

namespace WebApplication1.DTO.Responses
{
    public class TripResponseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }
        public CountryResponseDTO[] Countries { get; set; }
        public ClientResponseDTO[] Clients { get; set; }
    }
}