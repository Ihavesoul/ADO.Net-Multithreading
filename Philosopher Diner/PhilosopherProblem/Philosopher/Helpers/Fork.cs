using System;
using System.Collections.Generic;
using System.Text;

namespace Philosopher.Helpers
{
    public class Fork
    {
            //An int which represents the unique id of this fork, such as fork 1 or fork 2. 
            private int id;
            //The report object which prints messages to the Console, such as changes of state.
            private readonly Report _reportObject;

            public enum ForkMaterial
            {
                kSilver = 0,
                kGold = 1
            }

            public enum ForkActionTypes
            {
                kPickUp = 0,
                kPutDown = 1
            }

            public ForkMaterial m_forkMaterial { get; set; }



            /// <summary>
            /// Constructor for Fork objects.
            /// Initialises all fields for the Fork object.
            /// </summary>
            /// <param name="id">The unique id of this fork.</param>
            /// <param name="report">The report object which prints messages to the Console.</param>
            public Fork(int id, Report report)
            {
                this.id = id;
                this._reportObject = report;
            }


            public void ForkAction(string name, ForkActionTypes action)
            {
                String actionName = (action == ForkActionTypes.kPickUp) ? " has been picked up by " : " has been put down by ";
                String materialName = (m_forkMaterial == ForkMaterial.kGold) ? "Gold " : "Silver ";

                _reportObject.ReceiveMessage(materialName + "Fork " + id + actionName + name);
            }

            public ForkMaterial GetRandomMaterial()
            {

                Random random = new Random(Guid.NewGuid().GetHashCode());

                int materialNumber = random.Next(100);

                if (materialNumber <= 50)
                {
                    return ForkMaterial.kGold;
                }
                else
                {
                    return ForkMaterial.kSilver;
                }
            }

        }
    }
