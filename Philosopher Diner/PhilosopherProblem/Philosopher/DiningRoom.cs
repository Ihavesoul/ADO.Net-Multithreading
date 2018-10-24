using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Philosopher.Helpers;

namespace Philosopher
{
    /// <summary>
    /// A constructor for DiningRoom objects.
    /// This constructor initialises all of Forks and the Report object.
    /// It then uses Tasks to initialise all of the Philosophers,
    /// and start off their state transitions in parallel.
    /// </summary>
    public class DiningRoom
    {
        public DiningRoom(Report report)
        {
            //Fork to left of Plato and right of Galileo.

            Fork fork1 = new Fork(1, report);
            fork1.m_forkMaterial = fork1.GetRandomMaterial();

            //Fork to left of Confucius and right of Plato.
            Fork fork2 = new Fork(2, report);
            fork2.m_forkMaterial = fork1.GetRandomMaterial();

            //Fork to left of Aristotle and right of Confucius.
            Fork fork3 = new Fork(3, report);
            fork3.m_forkMaterial = fork1.GetRandomMaterial();

            //Fork to left of Socrates and right of Aristotle.
            Fork fork4 = new Fork(4, report);
            fork4.m_forkMaterial = fork1.GetRandomMaterial();

            //Fork to right of Socrates and left of Galileo.
            Fork fork5 = new Fork(5, report);
            fork5.m_forkMaterial = fork1.GetRandomMaterial();

            Task platoTask = new Task(() => new Philosopher("Plato", report, fork1, fork2));
            Task confuciusTask = new Task(() => new Philosopher("Confucius", report, fork2, fork3));
            Task aristotleTask = new Task(() => new Philosopher("Aristotle", report, fork3, fork4));
            Task socratesTask = new Task(() => new Philosopher("Socrates", report, fork4, fork5));
            Task galileoTask = new Task(() => new Philosopher("Galileo", report, fork5, fork1));

            platoTask.Start();
            confuciusTask.Start();
            aristotleTask.Start();
            socratesTask.Start();
            galileoTask.Start();

        }
    }
}
