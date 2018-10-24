using System;
using System.Threading;
using Philosopher.Helpers;

namespace Philosopher
{
    public class Philosopher
    {
        //The Random object used to supply random numbers for waiting varied amount of time before changing states.
        private Random rng = new Random();
        //The Report object that prints messages to the console.
        private Report reportObject;
        //The Fork object to the left of the Philosopher.
        private Fork leftFork;
        ////The Fork object to the right of the Philosopher
        private Fork rightFork;
        //A property referring to the name of the Philosopher. Getter and setter.
        private string Name { get; set; }
        //A property referring to the status (or state) of the Philosopher. Getter and private setter.
        public string Status { get; private set; }

        /// <summary>
        /// A constructor for Philosopher objects,
        /// initializes all fields,
        /// and starts off the continuous state transitioning
        /// of the Philosopher.
        /// </summary>
        /// <param name="name">The name of the Philosopher</param>
        /// <param name="report">The Report object that prints messages,
        /// such as change of states, to the Console.</param>
        /// <param name="leftFork">The Fork object to the left of the Philosopher.</param>
        /// <param name="rightFork">The Fork object to the right of the Philosopher.</param>
        public Philosopher(string name, Report report, Fork leftFork, Fork rightFork)
        {
            this.Name = name;
            this.reportObject = report;
            this.leftFork = leftFork;
            this.rightFork = rightFork;
            StateTransition();

        }

        /// <summary>
        /// A method that calls EnterThinkingState(),
        /// EnterHungryState(), (and EnterEatingState() from within
        /// that method),these represent the 3 states
        /// Philosophers can take, and then loops, causing a continuous transition
        /// between states. Only 2 methods are called in here as one of the states
        /// a Philosopher can take is called from inside a method representing another
        /// state.
        /// </summary>
        public void StateTransition()
        {
            while (true)
            {
                EnterThinkingState();
                Thread.Sleep(3000);
                EnterHungryState();
            }
        }


        /// <summary>
        /// A method that represents the thinking state of the Philosopher.
        /// Does various work that represents the thinking state, such as
        /// telling the Report object this Philosopher is thinking,
        /// and waiting a random amount of time before returning, 
        /// that is becoming hungry. (As that is the next state.)
        /// </summary>
        public void EnterThinkingState()
        {

            Thread.Sleep(rng.Next(1000) + 1);
            Status = "thinking";
            reportObject.ReceiveMessage(Name + " is " + Status);
            Thread.Sleep(rng.Next(1000) + 1);
        }

        /// <summary>
        /// A method that represents the hungry state of the Philosopher.
        /// Does various work that represents the hungry state, such as
        /// telling the Report object this Philosopher is hungry, 
        /// attempting to get a mutex on the left fork,
        /// after which the Philosopher attempts to get a mutex on the right fork.
        /// The Philosopher then loops over attempting to get a mutex on the left and 
        /// right fork until it is eventually successful. This is implemented
        /// via monitors and a while loop that only ends when two booleans
        /// that represent whether a mutex was successfully grabbed on each fork
        /// are true. This implementation SHOULD mean no deadlock is possible.
        /// Once the forks are successfully grabbed, this method then
        /// calls EnterEatingState() to transition the Philosopher to 
        /// the eating state, and once that returns (or that state ends)
        /// the Philosopher releases the mutexes on it's Forks.
        /// </summary>
        public void EnterHungryState()
        {
            Status = "hungry";
            reportObject.ReceiveMessage(Name + " is " + Status);
            bool leftForkTaken = false;
            bool rightForkTaken = false;
            while (!leftForkTaken && !rightForkTaken)
            {
                if (!leftForkTaken)
                {
                    Monitor.TryEnter(leftFork, 1000, ref leftForkTaken);
                    if (leftForkTaken)
                    {
                        leftFork.ForkAction(Name, Fork.ForkActionTypes.kPickUp);
                    }
                }
                if (!rightForkTaken)
                {
                    Monitor.TryEnter(rightFork, 1000, ref rightForkTaken);
                    if (rightForkTaken)
                    {
                        rightFork.ForkAction(Name, Fork.ForkActionTypes.kPickUp);
                    }
                }
            }
            EnterEatingState();
            Monitor.Exit(leftFork);
            Monitor.Exit(rightFork);
        }


        /// <summary>
        /// A method that represents the eating state of the Philosopher.
        /// Does various work that represents the eating state, such as
        /// telling the Report object this Philosopher is eating,
        /// and waiting a random amount of time before putting the forks down, 
        /// that is finishing eating and starting to think. (As that is the next state.)
        /// </summary>
        public void EnterEatingState()
        {
            Status = "eating";
            reportObject.ReceiveMessage(Name + " is " + Status);
            Thread.Sleep(rng.Next(1000) + 1);
            leftFork.ForkAction(Name, Fork.ForkActionTypes.kPutDown);
            rightFork.ForkAction(Name, Fork.ForkActionTypes.kPutDown);
        }

    }
}
