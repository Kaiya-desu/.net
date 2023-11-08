using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO.Requests
{
    public class ClientRequestDTO
    {
        // w tabelach atrybuty maja ustawione NOT NULL oraz nvarchar(120), stąd moje walidacje
        // dodatkowo waliduje adres email, numer telefonu i pesel, bo wedlug mnie powinny byc dodatkowo sprawdzane (tj nie powinny byc zwyklym stringiem)
        
        [Required(ErrorMessage = "Client's name is required!")]
        [MaxLength(120, ErrorMessage = "Text is over 120 characters.")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Client's surname is required!")]
        [MaxLength(120, ErrorMessage = "Text is over 120 characters.")]
        public string LastName {get; set; }
        
        [Required(ErrorMessage = "Client's e-mail is required!")]
        [MaxLength(120, ErrorMessage = "Text is over 120 characters.")]
        [EmailAddress(ErrorMessage = "Wrong format!")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Client's phone number is required!")]
        [Phone(ErrorMessage = "Wrong format!")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Client's pesel is required!")]
        [RegularExpression("\\d{11}", ErrorMessage = "Pesel should have exactly 11 numbers") ]
        public string Pesel { get; set; }

        // to dodajemy z route, po co po raz drugi wpisywac? 
        [Required(ErrorMessage = "IdTrip is required!")]
        [Range(1,Int32.MaxValue, ErrorMessage = "IdTrip should be greater than 0!")]
        public int IdTrip { get; set; }
        
        // tego i tak nie dodajemy do bazy, po co nam to? ( + powinnismy sprawdzac czy trip o takim id ma taka nazwe bo inaczej to jest bez sensu)
        [Required(ErrorMessage = "Trip name is required!")]
        [MaxLength(120, ErrorMessage = "Text is over 120 characters.")]
        public string TripName { get; set; }
       
        // DateTime moze byc null
        public DateTime? PaymentDate { get; set; }
    }
}