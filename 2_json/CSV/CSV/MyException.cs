using System;
using System.IO;
using CSV.Models;

namespace CSV
{
    public class MyException : Exception
    {
        public MyException ()
        {}

        public MyException (string message) 
            : base(message)
        {}

        public MyException (string message, Exception innerException)
            : base (message, innerException)
        {}    
        
    }
}