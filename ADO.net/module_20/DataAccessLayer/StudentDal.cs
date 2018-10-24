using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using module_20.Helpers;
using module_20.Models;

namespace module_20.DataAccessLayer
{
    public class StudentDal
    {
        private readonly IConfig _iConfig;
        private readonly ILogger _iLogger;

        public StudentDal(ILogger iLogger, IConfig iConfig)
        {
            _iLogger = iLogger;
            _iConfig = iConfig;
        }

        public StudentDal()
        {
        }

        public void InsertStudent(string firstName, string secondName, string email, string phoneNumber)
        {
            try
            {
                const string sqlQuery = "INSERT INTO student (firstName,secondName,email,phoneNumber) Values (@firstName,@secondName,@email,@phoneNumber)";
                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@firstName", firstName);
                        command.Parameters.AddWithValue("@secondName", secondName);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@phoneNumber", phoneNumber);

                        command.ExecuteNonQuery();
                    }
                }
            }
            //TODO: Create Custom Exeptions
            catch (SqlException e)
            {
                foreach (SqlError error in e.Errors)
                {
                    _iLogger.Log($"Exception line {error.LineNumber} + \n");
                    _iLogger.Log($"Exception message {error.Message} + \n");
                    _iLogger.Log($"Source  {error.Source} + \n");
                    _iLogger.Log($"Procedure {error.Procedure} + \n");
                }
            }

            catch (ArgumentNullException e)
            {
                _iLogger.Log($"Exception {e}: Your firstName, secondName, email, phoneNumber can not be null");
                throw;
            }
        }

        public void UpdateStudent(int studentId, string firstName, string secondName, string email, string phoneNumber)
        {
            try
            {
                const string updateQuery = "UPDATE Students SET firstName = @firstName,secondName = @secondName, email = @email, phoneNumber = @phoneNumber Where studentId=@studentId";

                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@firstName", firstName);
                        command.Parameters.AddWithValue("@secondName", secondName);
                        command.Parameters.AddWithValue("@studentId", studentId);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@phoneNumber", phoneNumber);

                        command.ExecuteNonQuery();
                    }
                }
            }
            //TODO: Create Custom Exeptions
            catch (SqlException e)
            {
                foreach (SqlError error in e.Errors)
                {
                    _iLogger.Log($"Exception line {error.LineNumber} + \n");
                    _iLogger.Log($"Exception message {error.Message} + \n");
                    _iLogger.Log($"Source  {error.Source} + \n");
                    _iLogger.Log($"Procedure {error.Procedure} + \n");
                }
            }

            catch (ArgumentNullException e)
            {
                _iLogger.Log($"Exception {e}: Your firstName, secondName, email, phoneNumber can not be null");
                throw;
            }
        }

        public Student GetStudentById(int studentId)
        {
            try
            {
                var student = new Student();

                const string sqlQuery = "SELECT firstName,secondName from Students where studentId=@studentId";

                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@studentId", studentId);

                        var dataReader = command.ExecuteReader();

                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (!Int32.TryParse(dataReader["studentId"].ToString(), out student.studentId.id))
                                {
                                    throw new FormatException(nameof(student.studentId));
                                }
                                student.firstName = dataReader["firstName"]?.ToString();
                                student.secondName = dataReader["secondName"]?.ToString();
                                student.email = dataReader["email"]?.ToString();
                                student.phoneNumber = dataReader["phoneNumber"]?.ToString();
                            }
                        }
                    }
                }

                return student;
            }
            //TODO: Create Custom Exeptions
            catch (SqlException e)
            {
                foreach (SqlError error in e.Errors)
                {
                    _iLogger.Log($"Exception line {error.LineNumber} + \n");
                    _iLogger.Log($"Exception message {error.Message} + \n");
                    _iLogger.Log($"Source  {error.Source} + \n");
                    _iLogger.Log($"Procedure {error.Procedure} + \n");
                    _iLogger.Log("Returned : null");
                }

                return null;
            }

            catch (ArgumentNullException e)
            {
                _iLogger.Log($"Exception {e}: Your firstName, secondName, email, phoneNumber can not be null");
                throw;
            }
        }

        public List<Student> GetStudents()
        {
            try
            {
                var result = new List<Student>();
                
                const string sqlQuery = "SELECT firstName,secondName from Students";

                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        var dataReader = command.ExecuteReader();

                        if (!dataReader.HasRows)
                        {
                            return result;
                        }

                        while (dataReader.Read())
                        {
                            var student = new Student();

                            if (Int32.TryParse(dataReader["studentId"].ToString(),out student.studentId.id))
                            {
                                throw new FormatException(nameof(student.studentId));
                            }
                            student.firstName = dataReader["firstName"]?.ToString();
                            student.secondName = dataReader["secondName"]?.ToString();
                            student.email = dataReader["email"]?.ToString();
                            student.phoneNumber = dataReader["phoneNumber"]?.ToString();
                            result.Add(student);
                        }
                    }
                }

                return result;
            }
            catch (SqlException e)
            {
                foreach (SqlError error in e.Errors)
                {
                    _iLogger.Log($"Exception line {error.LineNumber} + \n");
                    _iLogger.Log($"Exception message {error.Message} + \n");
                    _iLogger.Log($"Source  {error.Source} + \n");
                    _iLogger.Log($"Procedure {error.Procedure} + \n");
                    _iLogger.Log("Returned : null");
                }

                return null;
            }
        }

        public bool DeleteStudent(int studentId)
        {
            try
            {
                var result = false;
                const string sqlQuery = "DELETE FROM Students where studentId = @student";

                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@student", studentId);

                        var rowsDeletedCount = command.ExecuteNonQuery();
                        if (rowsDeletedCount != 0)
                        {
                            result = true;
                        }
                    }
                }

                return result;
            }
            catch (SqlException e)
            {
                foreach (SqlError error in e.Errors)
                {
                    _iLogger.Log($"Exception line {error.LineNumber} + \n");
                    _iLogger.Log($"Exception message {error.Message} + \n");
                    _iLogger.Log($"Source  {error.Source} + \n");
                    _iLogger.Log($"Procedure {error.Procedure} + \n");
                    _iLogger.Log("Returned : false");
                }

                return false;
            }
            catch (ArgumentNullException e)
            {
                _iLogger.Log($"Exception {e}: Your studentId can not be null");
                throw;
            }
        }
    }
}