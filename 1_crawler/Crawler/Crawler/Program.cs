using System;
using System.Collections.Generic;
using System.Linq; // standardowe biblioteki
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// namespace - przestrzenie nazw / pakiety
namespace Crawler
{
    public class Program
    {
        
        // Nazwy metod piszemy z dużej litery
        public static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentNullException("Brak argumentu wywołania!");
            }
           
                string url = args[0];
                var httpClient = new HttpClient();

                //Programowanie async vs parallel
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    HashSet<string> emailList = new HashSet<string>();
                    
                    if (response.IsSuccessStatusCode) //200
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        //regex na adresy email np pjatk@pja.edu.pl"

                        string pattern = @"[A-Z0-9]{2,}[._-]?[A-Z0-9]{2,}@[A-Z.A-Z]{2,}";
                        foreach (Match match in Regex.Matches(content, pattern, RegexOptions.IgnoreCase))
                        {
                            emailList.Add(match.Value);
                        }

                        if (emailList.Count > 0)
                        {
                            emailList.ToList<String>().ForEach(x => Console.WriteLine(x));
                        }
                        else
                        {
                            throw new Exception("Nie znaleziono adresów email");
                        }
                    }

                    httpClient.Dispose(); // mozna uzyc "using (var httpClient = new HttpClient())"
                }
                catch (InvalidOperationException e)
                {
                    throw new ArgumentException("Błędny adres URL");
                } 
                catch (HttpRequestException e)
                {
                    throw new Exception("Błąd w czasie pobierania strony");
                }
                
            }
        
    }
} 
