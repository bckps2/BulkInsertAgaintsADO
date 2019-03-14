using CommonLogicAlumnos;
using DAOLogic;
using System;
using System.Collections.Generic;

namespace BulkInsertVSADO
{
    class Program
    {
        static void Main(string[] args)
        {
            AlumnDAO alu = new AlumnDAO();
            List<Alumno> alumnos = new List<Alumno>();
            for (int i=0;i<2000000;i++) {

                alumnos.Add(new Alumno("registro"+i,"nuevo"+i,"AB"+i,1+i));
            }

            alu.Add(alumnos);

            alu.DELETETABLE();
        }
    }
}
