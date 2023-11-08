using System;
using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication
{
    // Nasza własna porównywarka studentów znajdujących się w pliku CSV
    public class OwnComparer : IEqualityComparer<Student>
    {
        public bool Equals(Student x, Student y)
        {
            return StringComparer
                .InvariantCultureIgnoreCase
                .Equals($"{x.Ska}",$"{y.Ska}");
        }

        public int GetHashCode(Student obj)
        {
            return StringComparer
                .CurrentCultureIgnoreCase
                .GetHashCode(
                    $"{obj.Ska}");
        }
    }
}
