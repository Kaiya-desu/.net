namespace CSV.Models
{
    public class Student
    {
        public string Ska { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string DataUrodzenia { get; set; }
        public string Email { get; set; }
        public string ImieMatki { get; set; }
        public string ImieOjca { get; set; }
        public Studies Studies { get; set; }
    }
}