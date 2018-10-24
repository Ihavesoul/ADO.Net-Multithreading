using System;
using module_20.BusinessLogic;
using module_20.DataAccessLayer;
using module_20.Helpers;
using module_20.Models;
using module_20.Models.IdStuctures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TestBuisinessLogic
{
    public class LogToConsole : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class Config : IConfig
    {
        public Config(string connectionString)
        {
            DbConnectionString = connectionString;
        }
        public string DbConnectionString { get; }
    }

    public class ConsoleSender : ISender
    {
        public virtual void SendMessage(string firstAdress, string secondAdress,string message)
        {
           Console.WriteLine(firstAdress,secondAdress,message);
        }
    }
    [TestClass]
    public class SenderTest
    {
        [TestMethod]
        public void SmsSenderTest()
        {
            var sender = new Mock<ConsoleSender>();   
            Mock<UniversityLogic> logic = new Mock<UniversityLogic>();
            //logic.Setup(x => x.SetMarkAndPresence(4, true, new Journal(new Student(new StudentIdStruct(3), "Josh","Beikfrot",1,2,"StudentEmail","StudentPhone"), new Lection(new LectionIdStruct(1),
               // new Lector(new LectionIdStruct(1),"LectorName","LectorSecondName" ,"LectorEmail"),"Ado.net",new DateTime(2018,12,24))), sender.Object));  
            var connectionString = "Data Source=DESKTOP-BP15CRQ\\SQL2017;Database = EpamStudents;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            var fakeConfig = new Config(connectionString);
            var fakeLogger = new LogToConsole();
            var logick = new UniversityLogic(fakeLogger, fakeConfig) ;
            logick.SetMarkAndPresence(4, true, new Journal(new Student(new StudentIdStruct(3), "Josh", "Beikfrot", 1, 2, "StudentEmail", "StudentPhone"), new Lection(new LectionIdStruct(1),
                new Lector(new LectionIdStruct(1), "LectorName", "LectorSecondName", "LectorEmail"), "Ado.net", new DateTime(2018, 12, 24))), sender.Object);
 
            sender.Verify(c => c.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }
    }
}
