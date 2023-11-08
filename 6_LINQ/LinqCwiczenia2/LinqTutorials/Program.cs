using System;

namespace LinqTutorials
{
    class Program
    {
        static void Main(string[] args)
        { 
           // var x = LinqTasks.Task13(new []{1,1,1,1,1,1,10,1,1,1,1}); Console.WriteLine(x);
            var t = LinqTasks.Task2();
            foreach (var emp in t)
                Console.WriteLine(emp);

        }
    }
}
