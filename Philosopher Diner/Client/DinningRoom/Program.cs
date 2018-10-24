using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Philosopher;
using Philosopher.Helpers;

namespace DinningRoom
{
    public class ConsoleMessageReceiver : ISenderBase
    {
        public void ReceiveMessage(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class DebugMessageReceiver : ISenderBase
    {
        public void ReceiveMessage(string message)
        {
            Debug.WriteLine(message);
        }
    }

    class StartSolution
    {
        static void Main(string[] args)
        {
            var repConsole = new Report(new ConsoleMessageReceiver());
            DiningRoom college = new DiningRoom(repConsole);
            Console.Read();
        }
    }
}
