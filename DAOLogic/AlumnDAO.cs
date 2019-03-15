using CommonLogicAlumnos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
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
            int numeroRegistros = 0;
            bool mas10 = false;
            var timeStart = DateTime.Now;

            using (var conn = new SqlConnection(connDB))
            {
                conn.Open();
                
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
                InsertText("Desde Ado ",timeStart,timeFinished,mas10,numeroRegistros);
                DeleteTable();
            } 
        }


        public void BulkCopy(List<Alumno> models)
        {
            var table = TransformToDataTable(models);
            using (var bulk = new SqlBulkCopy(connDB))
            {
                var timeStart = DateTime.Now;
                bulk.DestinationTableName = "TestAlumno1";
                bulk.WriteToServer(table);
                var timeFinished = DateTime.Now;
                InsertText("\nDesde Bulk ", timeStart, timeFinished, false, GetRowsCopied(bulk));
            }
            DeleteTable();
        }

       


        public DataTable TransformToDataTable(List<Alumno> models) {
            var table = new DataTable("TestAlumno1");

            table.Columns.Add(new DataColumn(("id"), typeof(int)));
            table.Columns.Add(new DataColumn("Name", typeof(String)));
            table.Columns.Add(new DataColumn("Surname", typeof(String)));
            table.Columns.Add(new DataColumn("DNI", typeof(String)));
            table.Columns.Add(new DataColumn("Age", typeof(int)));
            int i = 0;
            table.BeginLoadData();
            foreach (Alumno al in models)
            {
                i++;
                table.Rows.Add(i, al.Name, al.Surname, al.DNI, al.Age);
            }
            table.EndLoadData();
            return table;

        }


        public static int GetRowsCopied(SqlBulkCopy bulkCopy)
        {
            FieldInfo rowsCopiedField = null;
            if (rowsCopiedField == null)
            {
                rowsCopiedField = typeof(SqlBulkCopy).GetField("_rowsCopied", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            }
            return (int)rowsCopiedField.GetValue(bulkCopy);
        }


        public void InsertText(string tipoInsercion, DateTime start, DateTime finish, bool mas10, int registros)
        {
            using (StreamWriter strd = File.AppendText(@"C:/Users/user/desktop/logTimeInsert.txt"))
            {
                strd.WriteLine(tipoInsercion + " Corriendo la insercion " + start.ToString());
                if (mas10){
                    strd.WriteLine("Finalizando la inserción si tardó mas de diez minutos " + finish.ToString());
                }else{
                    strd.WriteLine("Finalizando la inserción" + finish.ToString());
                }
                strd.WriteLine("Timepo total de ejecución " + (finish - start).ToString() + " \nTotal registros = " + registros);
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
