using System.Collections.Generic;
using module_20.Models.IdStuctures;

namespace module_20.Models
{
    public class Lector
    {
        public Lector()
        {
        }

        public Lector(LectionIdStruct lectorId, string firstName, string secondName, string email)
        {
            this.lectorId = lectorId;
            this.firstName = firstName;
            this.secondName = secondName;
            this.email = email;
        }

        public LectionIdStruct lectorId;
        public string firstName;
        public string secondName;
        public string email;
    }
}