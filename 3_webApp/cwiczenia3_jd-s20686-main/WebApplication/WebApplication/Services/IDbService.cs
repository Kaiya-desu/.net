using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Services
{
    public interface IDbService
    {
        
        public void StringToStudentsSet(string values);
        public bool AddStudent(Student student);
        public HashSet<Student> Show();
        public void setMap(HashSet<Student> newSet);

    }
}