using System.Data.SqlClient;
using module_20.Helpers;

namespace module_20.BusinessLogic.Helpers
{
     static class DefaultDbGenerator
    {
        public static void GenerateDb(IConfig iConfig, ILogger iLogger)
        {
            using (var connection = new SqlConnection(iConfig.DbConnectionString))
            {
                var command = new SqlCommand(
                    "CREATE TABLE Students(" +
                    "studentId int IDENTITY(1, 1) NOT NULL," +
                    "firstName varchar(255) NOT NULL," +
                    "secondName varchar(255) NOT NULL," +
                    "averageMark int NOT NULL," +
                    "visiting float," +
                    "phoneNumber varchar(255)," +
                    "email varchar(255),"+
                    "CONSTRAINT[Students_PK] PRIMARY KEY NONCLUSTERED(studentId))" +
                    "GO" +
                    "CREATE TABLE Lectors(" +
                    "lector int  IDENTITY(1, 1)NOT NULL," +
                    "firstName varchar(255) NOT NULL," +
                    "secondName varchar(255) NOT NULL" +
                    "email varchar(255)," +
                    "CONSTRAINT[Lectors_PK] PRIMARY KEY NONCLUSTERED(lector))" +
                    "GO" +
                    "CREATE TABLE lection(" +
                    "lection int IDENTITY(1, 1) NOT NULL," +
                    "lector int NOT NULL," +
                    "LectionName varchar(255) NOT NULL," +
                    "LectionData datetime NOT NULL," +
                    "CONSTRAINT[Lection_PK] PRIMARY KEY NONCLUSTERED(lection)," +
                    "CONSTRAINT FK_Lection_Lectors FOREIGN KEY(lector)" +
                    "REFERENCES dbo.Lectors(lector)" +
                    "ON DELETE CASCADE" +
                    "ON UPDATE CASCADE" +
                    ");" +
                    "GO" +
                    "CREATE TABLE StudentAndLections(" +
                    "studentId int NOT NULL," +
                    "lection int NOT NULL," +
                    "mark int NOT NULL," +
                    "presence bit NOT NULL," +
                    "CONSTRAINT FK_Student_Lection_Lectors FOREIGN KEY (lection)" +
                    "REFERENCES dbo.lection(lection)" +
                    "ON DELETE CASCADE" +
                    "ON UPDATE CASCADE," +
                    "CONSTRAINT FK_Lection_Students FOREIGN KEY (studentId)" +
                    "REFERENCES dbo.Students(studentId)" +
                    "ON DELETE CASCADE" +
                    "ON UPDATE CASCADE," +
                    "CONSTRAINT[StudentAndLections_PK] PRIMARY KEY CLUSTERED (studentId ASC, lection ASC))" +
                    ");", connection);
            }
        }
    }
}