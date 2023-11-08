using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using CSV.Models;

namespace CSV
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // parametry: data/dane.csv output/output.json json -> korzystam z MacOS więc w ścieżkach mam / a nie \ (jak windows)

            if (args.Length == 0) throw new ArgumentNullException("Brak argumentu wywołania!");
            Regex rgx = new Regex(@"[A-Za-z0-9]+.csv");
            Match match = rgx.Match(args[0]);
            if (!File.Exists(args[0]))
                if (!match.Success)
                    throw new ArgumentException("Podana ścieżka jest niepoprawna");
                else
                    throw new FileNotFoundException("Plik nazwa nie istnieje");

            using var sw = new StreamWriter("log.txt");

            var studentSplit = new string[8]; // każdy student składa się z 9 elementów wiec tablica 0-8
            bool kompletneDane;
            
            var studentsSet = new HashSet<Student>(new OwnComparer());
            
            Student student = new Student();

           
                var pathIn = args[0];
                var fileIn = new FileInfo(pathIn);

                using (var stream1 = new StreamReader(fileIn.OpenRead()))
                {
                    string line = null;
                    while ((line = stream1.ReadLine()) != null)
                    {
                        studentSplit = line.Split(",");
                        //  Console.WriteLine(string.Join("; ", studentSplit)); // sprawdzanie czy poprawnie separuje

                        student = new Student
                        {
                            Ska = studentSplit[4],
                            Imie = studentSplit[0],
                            Nazwisko = studentSplit[1],
                            DataUrodzenia = studentSplit[5],
                            Email = studentSplit[6],
                            ImieMatki = studentSplit[7],
                            ImieOjca = studentSplit[8],
                            Studies = new Studies
                            {
                                Profil = studentSplit[2],
                                Tryb = studentSplit[3]
                            },
                        };
                        
                        try
                        {
                            kompletneDane = true;
                            for (int i = 0; i < studentSplit.Length; i++)
                            {
                                if (string.IsNullOrEmpty(studentSplit[i]))
                                {
                                    kompletneDane = false;
                                    break;
                                }
                            }

                            if (kompletneDane)
                            {
                                if (!studentsSet.Add(student)) 
                                {
                                    Console.WriteLine($"Student {student.Imie} {student.Nazwisko} znajduje się już w bazie.");
                                    sw.WriteLine($"Student {student.Imie} {student.Nazwisko} znajduje się już w bazie.");
                                }
                            }
                            else
                            {
                                throw new MyException();
                            }
                        }
                        catch (MyException e)
                        {
                            Console.Error.WriteLine($"Student {student.Imie} {student.Nazwisko} zawiera za mało danych"); 
                            sw.WriteLine($"Student {student.Imie} {student.Nazwisko} zawiera za mało danych");
                        }
                    }
                }

                // Console.WriteLine(studentsSet.Count);
                
                var date = DateTime.UtcNow.ToString("MM-dd-yyyy");
                Uczelnia uczelnia = new Uczelnia
                    {
                        CreatedAt = date, 
                        Author = "Karolina Struzek", 
                        Students = studentsSet
                    };
            
                JSON json = new JSON() {Uczelnia = uczelnia};
            
                var pathOut = args[1];
                using var jsonOut = new StreamWriter(pathOut);
                File.WriteAllText(pathOut,
                    JsonSerializer.Serialize(json)); //  JsonSerializer.Serialize(studentsSet) - zamiana na Json

        }
    }
}
