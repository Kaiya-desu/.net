namespace WebApplication.Models
{
    public class Student
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Ska { get; set; }
        public string DataUrodzenia { get; set; }
        public Studies Studies { get; set; }
        public string Email { get; set; }
        public string ImieMatki { get; set; }
        public string ImieOjca { get; set; }
        
        public override string ToString()
        {
            return Imie + "," + Nazwisko + "," + Ska + "," + DataUrodzenia + "," + Studies.Profil + "," + Studies.Tryb +
                   "," + Email + "," + ImieMatki + "," + ImieOjca;
        }
        
    }
}