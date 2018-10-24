using System;

namespace module_20.Helpers
{
    public interface ISender
    {
        void SendMessage(string firstAdress, string secondAdress,string message);
    }
}