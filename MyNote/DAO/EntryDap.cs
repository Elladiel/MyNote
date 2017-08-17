using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MyNote.Models;
using System.IO;

namespace MyNote.DAO
{
    public class EntryDap
    {
        static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Note;Integrated Security=True;Pooling=False";

        public static IEnumerable<Entry> GetEntries()
        {
            IEnumerable<Entry> entries = new List<Entry>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                entries = db.Query<Entry>("SELECT * FROM Entry");
            }
            return entries;
        }

        public static Entry Get(int id)
        {
            Entry entry = null;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                entry = db.Query<Entry>("SELECT * FROM Entry WHERE Id = @id", new { id }).FirstOrDefault();
            }
            return entry;
        }

        public static int Save(Entry entry)
        {
            int id;
            
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = @"IF exists(SELECT* FROM Entry WHERE Id = @Id) 
                                    begin
                                        update Entry set Date = @Date , TextEntry = @TextEntry where Id = @Id;
                                        select @Id;
                                    end
                                 ELSE 
                                    begin
                                    INSERT INTO Entry (Date, TextEntry) VALUES(@Date, @TextEntry);
                                    select scope_identity();
                                    end" ;
                id = db.Query<int>(sqlQuery, entry).Single();
            }
            return id;
        }

        public static void Create(Entry entry)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Entry (Date, TextEntry) VALUES(@Date, @TextEntry); SELECT CAST(SCOPE_IDENTITY() as int);";
               
                int entryId = db.Query<int>(sqlQuery, entry).FirstOrDefault();
                entry.Id = entryId;
            }
        }

        public static void Update(Entry entry)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Entry SET Date = @Date, TextEntry = @TextEntry WHERE Id = @Id";
                db.Execute(sqlQuery, entry);
            }
        }

        public static  void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Entry WHERE Id = @id";
                db.Execute(sqlQuery, new { id });
            }
        }

        public static void DeleteAll()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "truncate table Entry;";
                db.Execute(sqlQuery);
            }
        }

        private static void SaveFileToDatabase()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Images VALUES (@FileName, @Title, @ImageData)";
                command.Parameters.Add("@FileName", SqlDbType.NVarChar, 50);
                command.Parameters.Add("@Title", SqlDbType.NVarChar, 50);
                command.Parameters.Add("@ImageData", SqlDbType.Image, 1000000);

                // путь к файлу для загрузки
                string filename = @"C:\Users\Eugene\Pictures\cats.jpg";
                // заголовок файла
                string title = "Коты";
                // получаем короткое имя файла для сохранения в бд
                string shortFileName = filename.Substring(filename.LastIndexOf('\\') + 1); // cats.jpg
                                                                                           // массив для хранения бинарных данных файла
                byte[] imageData;
                using (System.IO.FileStream fs = new System.IO.FileStream(filename, FileMode.Open))
                {
                    imageData = new byte[fs.Length];
                    fs.Read(imageData, 0, imageData.Length);
                }
                // передаем данные в команду через параметры
                command.Parameters["@FileName"].Value = shortFileName;
                command.Parameters["@Title"].Value = title;
                command.Parameters["@ImageData"].Value = imageData;

                command.ExecuteNonQuery();
            }
        }
    }
}
