using System;
using System.Collections.Generic;
using CSV.Models;

namespace CSV
{
    // Nasza własna porównywarka studentów znajdujących się w pliku CSV
    public class OwnComparer : IEqualityComparer<Student>
    {
        public bool Equals(Student x, Student y)
        {
            // throw new System.NotImplementedException();
            return StringComparer
                .InvariantCultureIgnoreCase
                .Equals($"{x.Imie} {x.Nazwisko} {x.DataUrodzenia}",
                    $"{y.Imie} {y.Nazwisko} {y.DataUrodzenia}");
        }

        public int GetHashCode(Student obj)
        {
            // throw new System.NotImplementedException();
            return StringComparer
                .CurrentCultureIgnoreCase
                .GetHashCode(
                    $"{obj.Imie} {obj.Nazwisko} {obj.Ska}");
        }
    }
}
