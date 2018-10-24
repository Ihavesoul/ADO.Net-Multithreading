using System;
using module_20.Models.IdStuctures;

namespace module_20.Models
{
    public class Student : IEquatable<Student>
    {
        public Student()
        {
        }

        public Student(StudentIdStruct studentId, string firstName, string secondName)
        {
            this.studentId = studentId;
            this.firstName = firstName;
            this.secondName = secondName;
        }

        public Student(StudentIdStruct studentId, string firstName, string secondName, string email, string phoneNumber)
        {
            this.studentId = studentId;
            this.firstName = firstName;
            this.secondName = secondName;
            this.email = email;
            this.phoneNumber = phoneNumber;
        }

        public Student(StudentIdStruct studentId, string firstName, string secondName, int averageMark, int visiting, string email,
            string phoneNumber)
        {
            this.studentId = studentId;
            this.firstName = firstName;
            this.secondName = secondName;
            this.averageMark = averageMark;
            this.visiting = visiting;
            this.email = email;
            this.phoneNumber = phoneNumber;
        }

        public StudentIdStruct studentId;
        public string firstName;
        public string secondName;
        public int averageMark;
        public int visiting;
        public string email;
        public string phoneNumber;

        public bool Equals(Student other)
        {
            if (other == null)
            {
                return false;
            }

            return (this.studentId.id == other.studentId.id);
        }
    }
}