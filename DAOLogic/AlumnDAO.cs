using CommonLogicAlumnos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace DAOLogic
{
    public class AlumnDAO
    {
        string connDB;
        public AlumnDAO()
        {
            
            connDB = ConfigHelper.Config["ConnDB"];
        }

        public void Add(List<Alumno> models)
        {

            using (var conn = new SqlConnection(connDB))
            {
                conn.Open();
                int numeroRegistros=0;
                bool mas10=false;
                var timeStart = DateTime.Now;
                foreach (Alumno model in models)
                {
                    using (var command = new SqlCommand("INSERT INTO TestAlumno1(Name,Surname, DNI,Age) values(@Name, @Surname, @DNI, @Age)", conn))
                    {
                        command.Parameters.AddWithValue("@Name", model.Name);
                        command.Parameters.AddWithValue("@Surname", model.Surname);
                        command.Parameters.AddWithValue("@DNI", model.DNI);
                        command.Parameters.AddWithValue("@Age", model.Age);
                        command.ExecuteNonQuery();
                        if ((DateTime.Now - timeStart) > TimeSpan.FromMinutes(10)) {
                            mas10 = true;
                            break;
                        }
                        numeroRegistros++;
                    }
                }

                var timeFinished = DateTime.Now;
                InsertText(timeStart,timeFinished,mas10,numeroRegistros);
                } 
        }

        public void InsertText(DateTime start,DateTime finish, bool mas10,int registros) {
            using (StreamWriter strd = new StreamWriter(@"C:/Users/user/desktop/logTimeInsert.txt"))
            {
                strd.WriteLine("Corriendo la insercion " + start.ToString());
                if (mas10)
                {
                    strd.WriteLine("Finalizando la inserción si tardó mas de diez minutos " + finish.ToString());
                }
                else {
                    strd.WriteLine("Finalizando la inserción" + finish.ToString());
                }

                strd.WriteLine("Timepo total de ejecución" + (finish - start).ToString() + " \nTotal registros = " +registros);
            }
        }




        public void DeleteTable() {
            using (var conn = new SqlConnection(connDB))
            {
                conn.Open();
                using (var command = new SqlCommand("TRUNCATE TABLE TestAlumno1", conn))
                {
                    command.ExecuteNonQuery();

                }
            }


        }
    }
}
