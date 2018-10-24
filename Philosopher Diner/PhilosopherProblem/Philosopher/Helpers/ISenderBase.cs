using System;
using System.Collections.Generic;
using System.Text;

namespace Philosopher.Helpers
{
    public interface ISenderBase
    {
        void ReceiveMessage(string message);
    }
}
