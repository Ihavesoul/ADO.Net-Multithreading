using System;
using System.Collections.Generic;
using System.Text;

namespace Philosopher.Helpers
{

    public class Report 
    {
        private readonly ISenderBase _iSenderBase;

        public Report(ISenderBase iSenderBase)
        {
            _iSenderBase = iSenderBase;
        }

        public void ReceiveMessage(String arg)
        {
            // Do something here
            _iSenderBase.ReceiveMessage(arg);
        }
    }
}
