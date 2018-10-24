using System;
using module_20.Models.IdStuctures;

namespace module_20.Models
{
    public class Lection
    {
        public Lection()
        {
        }

        public Lection(Lector lector)
        {
            this.lector = lector;
        }

        public Lection(LectionIdStruct lectionId, Lector lector, string name, DateTime date)
        {
            this.lectionId = lectionId;
            this.lector = lector;
            this.name = name;
            this.date = date;
        }

        public LectionIdStruct lectionId ;
        public Lector lector;
        public string name;
        public DateTime date;
    }
}