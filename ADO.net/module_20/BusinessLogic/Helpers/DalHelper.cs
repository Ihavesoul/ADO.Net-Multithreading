using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using module_20.Models;

namespace module_20.BusinessLogic.Helpers
{
    static class DalHelper
    {
        public static Student GetStudent(this SqlDataReader dataReader)
        {
            var student = new Student();
            if (!int.TryParse(dataReader["studentId"].ToString(), out student.studentId.id))
            {
                throw new FormatException(nameof(student.studentId));
            }

            student.firstName = dataReader["firstName"]?.ToString();
            student.secondName = dataReader["secondName"]?.ToString();
            student.email = dataReader["email"]?.ToString();
            student.phoneNumber = dataReader["phoneNumber"]?.ToString();

            return student;
        }

        public static Lector GetLector(this SqlDataReader dataReader)
        {
            var lector = new Lector();
            if (!Int32.TryParse(dataReader["lectorId"]?.ToString(), out lector.lectorId.id))
            {
                throw new FormatException(nameof(lector.lectorId.id));
            }

            lector.firstName = dataReader["lectorFirstName"]?.ToString();
            lector.secondName = dataReader["lectorSecondName"]?.ToString();
            lector.email = dataReader["email"]?.ToString();

            return lector;
        }

        public static Lection GetLection(this SqlDataReader dataReader)
        {
            var lection = new Lection(GetLector(dataReader));
            if (!int.TryParse(dataReader["lectionId"]?.ToString(), out lection.lectionId.id))
            {
                throw new FormatException(
                    $"{lection.lectionId.id} can not be pars" + nameof(lection.lectionId));
            }

            if (!int.TryParse(dataReader["lectorId"]?.ToString(), out lection.lector.lectorId.id))
            {
                throw new FormatException($"{lection.lector.lectorId.id} can not be pars" +
                                          nameof(lection.lector));
            }

            lection.name = dataReader["LectionName"]?.ToString();

            if (!DateTime.TryParse(dataReader["LectionData"]?.ToString(), out lection.date))
            {
                throw new FormatException($"{lection.date} can not be pars" + nameof(lection.date));
            }

            return lection;
        }
    }
}
