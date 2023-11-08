using LinqTutorials.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace LinqTutorials
{
    public static class LinqTasks
    {
        public static IEnumerable<Emp> Emps { get; set; }
        public static IEnumerable<Dept> Depts { get; set; }

        static LinqTasks()
        {
            var empsCol = new List<Emp>();
            var deptsCol = new List<Dept>();

            #region Load depts

            var d1 = new Dept
            {
                Deptno = 1,
                Dname = "Research",
                Loc = "Warsaw"
            };

            var d2 = new Dept
            {
                Deptno = 2,
                Dname = "Human Resources",
                Loc = "New York"
            };

            var d3 = new Dept
            {
                Deptno = 3,
                Dname = "IT",
                Loc = "Los Angeles"
            };

            deptsCol.Add(d1);
            deptsCol.Add(d2);
            deptsCol.Add(d3);
            Depts = deptsCol;

            #endregion

            #region Load emps

            var e1 = new Emp
            {
                Deptno = 1,
                Empno = 1,
                Ename = "Jan Kowalski",
                HireDate = DateTime.Now.AddMonths(-5),
                Job = "Backend programmer",
                Mgr = null,
                Salary = 2000
            };

            var e2 = new Emp
            {
                Deptno = 1,
                Empno = 20,
                Ename = "Anna Malewska",
                HireDate = DateTime.Now.AddMonths(-7),
                Job = "Frontend programmer",
                Mgr = e1,
                Salary = 4000
            };

            var e3 = new Emp
            {
                Deptno = 1,
                Empno = 2,
                Ename = "Marcin Korewski",
                HireDate = DateTime.Now.AddMonths(-3),
                Job = "Frontend programmer",
                Mgr = null,
                Salary = 5000
            };

            var e4 = new Emp
            {
                Deptno = 2,
                Empno = 3,
                Ename = "Paweł Latowski",
                HireDate = DateTime.Now.AddMonths(-2),
                Job = "Frontend programmer",
                Mgr = e2,
                Salary = 5500
            };

            var e5 = new Emp
            {
                Deptno = 2,
                Empno = 4,
                Ename = "Michał Kowalski",
                HireDate = DateTime.Now.AddMonths(-2),
                Job = "Backend programmer",
                Mgr = e2,
                Salary = 5500
            };

            var e6 = new Emp
            {
                Deptno = 2,
                Empno = 5,
                Ename = "Katarzyna Malewska",
                HireDate = DateTime.Now.AddMonths(-3),
                Job = "Manager",
                Mgr = null,
                Salary = 8000
            };

            var e7 = new Emp
            {
                Deptno = null,
                Empno = 6,
                Ename = "Andrzej Kwiatkowski",
                HireDate = DateTime.Now.AddMonths(-3),
                Job = "System administrator",
                Mgr = null,
                Salary = 7500
            };

            var e8 = new Emp
            {
                Deptno = 2,
                Empno = 7,
                Ename = "Marcin Polewski",
                HireDate = DateTime.Now.AddMonths(-3),
                Job = "Mobile developer",
                Mgr = null,
                Salary = 4000
            };

            var e9 = new Emp
            {
                Deptno = 2,
                Empno = 8,
                Ename = "Władysław Torzewski",
                HireDate = DateTime.Now.AddMonths(-9),
                Job = "CTO",
                Mgr = null,
                Salary = 12000
            };

            var e10 = new Emp
            {
                Deptno = 2,
                Empno = 9,
                Ename = "Andrzej Dalewski",
                HireDate = DateTime.Now.AddMonths(-4),
                Job = "Database administrator",
                Mgr = null,
                Salary = 9000
            };

            empsCol.Add(e1);
            empsCol.Add(e2);
            empsCol.Add(e3);
            empsCol.Add(e4);
            empsCol.Add(e5);
            empsCol.Add(e6);
            empsCol.Add(e7);
            empsCol.Add(e8);
            empsCol.Add(e9);
            empsCol.Add(e10);
            Emps = empsCol;

            #endregion
        }

        /// <summary>
        ///     SELECT * FROM Emps WHERE Job = "Backend programmer";
        /// </summary>
        public static IEnumerable<Emp> Task1()
        {
            IEnumerable<Emp> resultSyntax = (from emp in Emps 
                                        where emp.Job == "Backend programmer"
                                        select emp).ToList();
            
            IEnumerable<Emp> result = Emps.Where(emp => emp.Job == "Backend programmer");
            return result;
        }

        /// <summary>
        ///     SELECT * FROM Emps Job = "Frontend programmer" AND Salary>1000 ORDER BY Ename DESC;
        /// </summary>
        public static IEnumerable<Emp> Task2()
        {
             IEnumerable<Emp> resultSyntax = (from emp in Emps 
                                        where emp.Job == "Frontend programmer" && emp.Salary > 1000
                                        orderby emp.Ename descending
                                        select emp).ToList();
            
            IEnumerable<Emp> result = Emps.Where(emp => emp.Job == "Frontend programmer" && emp.Salary > 1000)
                                            .OrderByDescending(emp => emp.Ename);
            return result;
        }


        /// <summary>
        ///     SELECT MAX(Salary) FROM Emps;
        /// </summary>
        public static int Task3()
        {
            int resultSyntax = (from emp in Emps 
                                select emp.Salary).Max();
            
            int result = Emps.Max(emp => emp.Salary);
            return result;
        }

        /// <summary>
        ///     SELECT * FROM Emps WHERE Salary=(SELECT MAX(Salary) FROM Emps);
        /// </summary>
        public static IEnumerable<Emp> Task4()
        {
            IEnumerable<Emp> resultSyntax = (from emp in Emps 
                                            where emp.Salary == (from e in Emps select e.Salary).Max()
                                            select emp);
            
            IEnumerable<Emp> result = Emps.Where(emp => emp.Salary == Emps.Max(emp => emp.Salary));
            return result;
        }

        /// <summary>
        ///    SELECT ename AS Nazwisko, job AS Praca FROM Emps;
        /// </summary>
        public static IEnumerable<object> Task5()
        {
            
            IEnumerable<object> resultSyntax = (from emp in Emps  
                                            select new
                                            {
                                                Nazwisko = emp.Ename,
                                                Praca = emp.Job
                                            }).ToList();
            
            IEnumerable<object> result = Emps.Select(emp => new
                                                    {
                                                        Nazwisko = emp.Ename,
                                                        Praca = emp.Job
                                                    }).ToList();
            return result;
        }

        /// <summary>
        ///     SELECT Emps.Ename, Emps.Job, Depts.Dname FROM Emps
        ///     INNER JOIN Depts ON Emps.Deptno=Depts.Deptno
        ///     Rezultat: Złączenie kolekcji Emps i Depts.
        /// </summary>
        public static IEnumerable<object> Task6()
        {
            IEnumerable<object> resultSyntax = (from emp in Emps 
                                            join dept in Depts on emp.Deptno equals dept.Deptno
                                            select new
                                            {
                                                emp.Ename,
                                                emp.Job,
                                                dept.Dname,
                                            } ).ToList(); 
            
            IEnumerable<object> result = Emps.Join(Depts, emp => emp.Deptno, dept => dept.Deptno, 
                                        (emp,dept) => new{Emps = emp, Depts = dept})
                                                    .Select(x => new {x.Emps.Ename, x.Emps.Job, x.Depts.Dname}).ToList();
            return result;
        }

        /// <summary>
        ///     SELECT Job AS Praca, COUNT(1) LiczbaPracownikow FROM Emps GROUP BY Job;
        /// </summary>
        public static IEnumerable<object> Task7()
        {
            IEnumerable<object> resultSyntax = (from emp in Emps
                                            group emp by emp.Job into praca
                                            select new
                                            {
                                                Praca = praca.Key,
                                                LiczbaPracownikow = praca.Count()
                                           }).ToList();
            
            
            IEnumerable<object> result = Emps.GroupBy(emp => emp.Job)
                                        .Select(g => new
                                            {
                                                Praca = g.Key,
                                                LiczbaPracownikow = g.Count()
                                            }).ToList();
            return result;
        }

        /// <summary>
        ///     Zwróć wartość "true" jeśli choć jeden
        ///     z elementów kolekcji pracuje jako "Backend programmer".
        /// </summary>
        public static bool Task8()
        {
            bool resultSyntax = (from emp in Emps
                            where emp.Job == "Backend programmer"
                            select emp).Any();
            
            bool result = Emps.Any(emp => emp.Job == "Backend programmer");
            return result;
        }

        /// <summary>
        ///     SELECT TOP 1 * FROM Emp WHERE Job="Frontend programmer"
        ///     ORDER BY HireDate DESC;
        /// </summary>
        public static Emp Task9()
        {
            Emp resultSyntax = (from emp in Emps
                where emp.Job == "Frontend programmer" 
                orderby emp.HireDate descending
                select emp).FirstOrDefault();
            
            Emp result = Emps.Where(emp => emp.Job == "Frontend programmer")
                            .OrderByDescending(emp => emp.HireDate).FirstOrDefault();
            return result;
        }

        /// <summary>
        ///     SELECT Ename, Job, Hiredate FROM Emps
        ///     UNION
        ///     SELECT "Brak wartości", null, null;
        /// </summary>
        public static IEnumerable<object> Task10()
        {
            var empty = new List<Emp>();
            empty.Add(new Emp()
            {
                Ename = "Brak wartości",
                Job = null,
                HireDate = null
            });

            IEnumerable<object>  resultSyntax = (from emp in Emps 
                                        select new
                                        {
                                            emp.Ename,
                                            emp.Job,
                                            emp.HireDate
                                        })
                                        .Union(from emp in empty 
                                        select new
                                        {
                                            emp.Ename,
                                            emp.Job,
                                            emp.HireDate
                                        });
            
            IEnumerable<object>  result = Emps.Select(emp => new
                                         {
                                            emp.Ename,
                                            emp.Job,
                                            emp.HireDate
                                        })
                                        .Union(empty.Select(emp => new
                                        {
                                            emp.Ename,
                                            emp.Job,
                                            emp.HireDate
                                        }));
            return result;
        }

        /// <summary>
        /// Wykorzystując LINQ pobierz pracowników podzielony na departamenty pamiętając, że:
        /// 1. Interesują nas tylko departamenty z liczbą pracowników powyżej 1
        /// 2. Chcemy zwrócić listę obiektów o następującej srukturze:
        ///    [
        ///      {name: "RESEARCH", numOfEmployees: 3},
        ///      {name: "SALES", numOfEmployees: 5},
        ///      ...
        ///    ]
        /// 3. Wykorzystaj typy anonimowe
        /// </summary>
        public static IEnumerable<object> Task11()
        {
            IEnumerable<object> resultSyntax = (from emp in Emps 
                join dept in Depts on emp.Deptno equals dept.Deptno
                group dept by dept.Dname into grouping
                where grouping.Count() > 1
                select new
                {
                    name = grouping.Key,
                    numOfEmployees = grouping.Count()
                }).ToList(); 
            
            IEnumerable<object> result = Emps.Join(Depts, emp => emp.Deptno, dept => dept.Deptno, 
                    (emp,dept) => new{Emps = emp, Depts = dept})
                    .GroupBy(emp => emp.Depts.Dname)
                    .Where(grouping => grouping.Count() > 1)
                    .Select(grouping => new
                        {
                            name = grouping.Key,
                            numOfEmployees = grouping.Count()
                        }).ToList();;
            return result;
        }

        /// <summary>
        /// Napisz własną metodę rozszerzeń, która pozwoli skompilować się poniższemu fragmentowi kodu.
        /// Metodę dodaj do klasy CustomExtensionMethods, która zdefiniowana jest poniżej.
        /// 
        /// Metoda powinna zwrócić tylko tych pracowników, którzy mają min. 1 bezpośredniego podwładnego.
        /// Pracownicy powinny w ramach kolekcji być posortowani po nazwisku (rosnąco) i pensji (malejąco).
        /// </summary>
        public static IEnumerable<Emp> Task12()
        {
            IEnumerable<Emp> result = CustomExtensionMethods.GetEmpsWithSubordinates(Emps);
            return result;
        }

        /// <summary>
        /// Poniższa metoda powinna zwracać pojedyczną liczbę int.
        /// Na wejściu przyjmujemy listę liczb całkowitych.
        /// Spróbuj z pomocą LINQ'a odnaleźć tę liczbę, które występuja w tablicy int'ów nieparzystą liczbę razy.
        /// Zakładamy, że zawsze będzie jedna taka liczba.
        /// Np: {1,1,1,1,1,1,10,1,1,1,1} => 10
        /// </summary>
        public static int Task13(int[] arr)
        {
            var resultSyntax = (from x in arr
                group x by x into grouping
                where grouping.Count()%2 != 0
                select new
                {
                    grouping.Key
                }.Key).Max(); 
            
            var result = arr.GroupBy(x => x)
                .Where(x => x.Count()%2 != 0)
                .Select(x => x.Key).Max();

            return result;
        }

        /// <summary>
        /// Zwróć tylko te departamenty, które mają 5 pracowników lub nie mają pracowników w ogóle.
        /// Posortuj rezultat po nazwie departament rosnąco.
        /// </summary>
        public static IEnumerable<Dept> Task14()
        {
            // SELECT d.DEPTNO FROM DEPT d
            // WHERE 5==(SELECT COUNT(*) FROM EMP e WHERE d.DEPTNO=e.DEPTNO)
            // OR (NOT EXISTS(SELECT 1 FROM EMP e WHERE d.DEPTNO=e.DEPTNO)) ORDER BY D.DEPTNO;
            var resultSyntax = (from dept in Depts
                    from emp in Emps
                    where dept.Deptno == emp.Deptno
                    group dept by dept into grouping
                    where grouping.Count() == 5
                    select grouping.Key)
                .Union(from dept in Depts from emp in Emps where Emps.All(e => e.Deptno != dept.Deptno) select dept);

            var result = Depts
                .Where(dept => Emps.Count(emp => emp.Deptno == dept.Deptno) == 5 || Emps.All(emp => emp.Deptno != dept.Deptno))
                .OrderBy(dept => dept.Deptno).ToList();
            
            return result;
        }
    }
    
    public static class CustomExtensionMethods
    {
        //Put your extension methods here
        public static IEnumerable<Emp> GetEmpsWithSubordinates(this IEnumerable<Emp> emps)
        {
            var resultSyntax = (from emp in emps
                where emps.Any(e => e.Mgr != null && emp.Empno == e.Mgr.Empno)
                orderby emp.Ename ascending, emp.Salary descending
                select emp);
            
            var result = emps.Where(emp => emps.Any(e => e.Mgr != null && emp.Empno == e.Mgr.Empno))
                                                    .OrderBy(emp => emp.Ename).ThenByDescending(emp => emp.Salary);
            return result;
        }

    }
}
