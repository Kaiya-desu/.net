using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class DbService : IDbService
    {
        
        // plik CSV : imie, nazwisko, numerIndeksu, dataUrodzenia, studia, tryb, email, imię ojca, imię matki
        private string path = "/Users/kaiya/cwiczenia3_jd-s20686/WebApplication/WebApplication/data/dane.csv";
        
        HashSet<Student> studentsSet = new HashSet<Student>(new OwnComparer());

        public DbService()
        {
            Regex rgx = new Regex(@"[A-Za-z0-9]+.csv");
            Match match = rgx.Match(path);
            if (!File.Exists(path))
                if (!match.Success)
                    throw new ArgumentException("Podana ścieżka jest niepoprawna");
                else
                    throw new FileNotFoundException("Plik nazwa nie istnieje");

            var fileIn = new FileInfo(path);

            using (var stream1 = new StreamReader(fileIn.OpenRead()))
            {
                string line = null;
                while ((line = stream1.ReadLine()) != null)
                {
                    StringToStudentsSet(line);
                }

            }
        }

        string[] studentSplit = new string[9]; // każdy student składa się z 9 elementów
        Student student;
        bool kompletneDane;

        public void StringToStudentsSet(string values)
        {
            studentSplit = values.Split(","); 
            
                student = new Student
                {
                    Imie = studentSplit[0],
                    Nazwisko = studentSplit[1],
                    Ska = studentSplit[2],
                    DataUrodzenia = studentSplit[3],
                    Studies = new Studies
                    {
                        Profil = studentSplit[4],
                        Tryb = studentSplit[5]
                    },
                    Email = studentSplit[6],
                    ImieMatki = studentSplit[7],
                    ImieOjca = studentSplit[8],
                };
                
                AddStudent(student);
        }

        Regex skaValidator = new (@"s[0-9]{1,}");
        Match match;
        string values;
        public bool AddStudent(Student student)
        {
            
            // bo AddStudent działa też bez metody StringToStudentsSet
            values = student.ToString();
            studentSplit = values.Split(","); 
            
            kompletneDane = true;
            
                for (int i = 0; i < studentSplit.Length; i++)
                {
                    if (string.IsNullOrEmpty(studentSplit[i]))
                    {
                        kompletneDane = false;
                        break;
                    }
                }

                match = skaValidator.Match(student.Ska); // sprawdzam czy s-ka jest prawidłowa

                if (kompletneDane && match.Success)
                {
                    if (!studentsSet.Add(student))
                    {
                        // student o danej S-ce znajduje sie już w bazie
                        return false;
                    }
                }
                else
                {
                    // niekompletne dane
                    return false;
                }
            // gdy student zostanie dodany 
            return true;
        }

        public HashSet<Student> Show()
        {
            return studentsSet;
        }

        public void setMap(HashSet<Student>newSet)
        {
            studentsSet = newSet;
            // zapisanie do pliku csv
            using var csv = new StreamWriter(path);
            File.WriteAllText(@path,String.Empty, Encoding.UTF8); // wyczyszczenie pliku
            studentsSet.ToList<Student>().ForEach(x => csv.WriteLine(x));
        }

    }
}