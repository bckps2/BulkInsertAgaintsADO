using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLogicAlumnos
{
    public class Alumno
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DNI { get; set; }
        public int Age { get; set; }

        public Alumno(string name,string surname,string dni,int age){
            this.Name = name;
            this.Surname = surname;
            this.DNI = dni;
            this.Age = age;
        }
    }
}
