using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using module_20.BusinessLogic.Helpers;
using module_20.Helpers;
using module_20.Models;

namespace module_20.DataAccessLayer
{
    public class JournalDal
    {
        private readonly IConfig _iConfig;
        private readonly ILogger _iLogger;

        public JournalDal(ILogger iLogger, IConfig iConfig)
        {
            _iLogger = iLogger;
            _iConfig = iConfig;
        }

        public void InsertStudentAndLection(int studentId, int lectionId, int mark, bool presence, bool homework)
        {
            try
            {
                const string sqlQuery =
                    "INSERT INTO StudentAndLections (studentId,lectionId,mark,presence,homework) Values (@studentId,@lectionId,@mark,@presence,@homework)";
                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@studentId", studentId);
                        command.Parameters.AddWithValue("@lectionId", lectionId);
                        command.Parameters.AddWithValue("@mark", mark);
                        command.Parameters.AddWithValue("@presence", presence);
                        command.Parameters.AddWithValue("@homework", homework);
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
                _iLogger.Log($"Exception {e}: Your studentId, lectionId,mark,presence , homework can not be null");
                throw;
            }
        }

        public void UpdateStudentAndLection(int studentId, int lectionId, int mark, bool presence, bool homework)
        {
            try
            {
                const string updateQuery =
                    "UPDATE StudentAndLections SET studentId = @studentId,lectionId = @lectionId, mark = @mark, presence = @presence, homework = @homework Where studentId = @studentId";

                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@studentId", studentId);
                        command.Parameters.AddWithValue("@lectionId", lectionId);
                        command.Parameters.AddWithValue("@mark", mark);
                        command.Parameters.AddWithValue("@presence", presence);
                        command.Parameters.AddWithValue("@homework", homework);
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
                _iLogger.Log($"Exception {e}: Your studentId, lectionId,mark,presence , homework can not be null");
                throw;
            }
        }

        public Journal GetStudentAndLectionById(int studentId,int lectionId)
        {
            try
            {
                const string sqlQuery =
                    "SELECT sal.studentId,sal.lectionId,sal.mark,sal.presence,sal.homework," +
                    "S.firstName,S.secondName,S.email,S.phoneNumber," +
                    "L.lectionName ,L.lectionData," +
                    "LRS.firstName lectorFirstName,LRS.secondName lectorSecondName,LRS.lectorId " +
                    "From StudentAndLections sal " +
                    "Where S.studentId = @studentId,L.lectionId = @lectionId"+
                    "JOIN Lection L ON sal.lectionId = L.lectionId " +
                    "LEFT JOIN Students S ON sal.studentId = S.studentId  " +
                    "JOIN Lectors LRS ON L.lectorId  = LRS.lectorId";

                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@studentId", studentId);
                        command.Parameters.AddWithValue("@lectionId", lectionId);

                        var dataReader = command.ExecuteReader();

                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                var student = dataReader.GetStudent();
                                var lection = dataReader.GetLection();

                                var studentAndLection = new Journal(student, lection);
                                if (!int.TryParse(dataReader["studentId"].ToString(),
                                    out studentAndLection.student.studentId.id))
                                {
                                    throw new FormatException(nameof(studentAndLection.student.studentId.id));
                                }

                                if (!int.TryParse(dataReader["lectionId"].ToString(),
                                    out studentAndLection.lection.lectionId.id))
                                {
                                    throw new FormatException(nameof(studentAndLection.lection.lectionId.id));
                                }

                                if (!int.TryParse(dataReader["mark"].ToString(), out studentAndLection.mark))
                                {
                                    throw new FormatException(nameof(studentAndLection.mark));
                                }

                                if (!bool.TryParse(dataReader["presence"].ToString(),
                                    out studentAndLection.presence))
                                {
                                    throw new FormatException(nameof(studentAndLection.presence));
                                }

                                if (!bool.TryParse(dataReader["homework"].ToString(),
                                    out studentAndLection.homework))
                                {
                                    throw new FormatException(nameof(studentAndLection.homework));
                                }

                                return studentAndLection;
                            }
                        }
                    }
                }

                return null;
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

            catch (ArgumentNullException e)
            {
                _iLogger.Log($"Exception {e}: Your studentId, lectionId can not be null");
                throw;
            }
        }

        public List<Journal> GetStudentAndLectionsWithStudentMark()
        {
            try
            {
                var result = new List<Journal>();

                const string sqlQuery =
                    "SELECT sal.studentId,sal.lectionId,sal.mark,sal.presence,sal.homework," +
                    "S.firstName,S.secondName,S.email,S.phoneNumber," +
                    "L.lectionName ,L.lectionData," +
                    "LRS.firstName lectorFirstName,LRS.secondName lectorSecondName,LRS.lectorId " +
                    "From StudentAndLections sal " +
                    "JOIN Lection L ON sal.lectionId = L.lectionId " +
                    "LEFT JOIN Students S ON sal.studentId = S.studentId  " +
                    " JOIN Lectors LRS ON L.lectorId  = LRS.lectorId";

                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        var dataReader = command.ExecuteReader();

                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                var student = dataReader.GetStudent();
                                var lection = dataReader.GetLection();

                                var studentAndLection = new Journal(student, lection);
                                if (!int.TryParse(dataReader["studentId"].ToString(),
                                    out studentAndLection.student.studentId.id))
                                {
                                    throw new FormatException(nameof(studentAndLection.student.studentId.id));
                                }

                                if (!int.TryParse(dataReader["lectionId"].ToString(),
                                    out studentAndLection.lection.lectionId.id))
                                {
                                    throw new FormatException(nameof(studentAndLection.lection.lectionId.id));
                                }

                                if (!int.TryParse(dataReader["mark"].ToString(), out studentAndLection.mark))
                                {
                                    throw new FormatException(nameof(studentAndLection.mark));
                                }

                                if (!bool.TryParse(dataReader["presence"].ToString(),
                                    out studentAndLection.presence))
                                {
                                    throw new FormatException(nameof(studentAndLection.presence));
                                }

                                if (!bool.TryParse(dataReader["homework"].ToString(),
                                    out studentAndLection.homework))
                                {
                                    throw new FormatException(nameof(studentAndLection.homework));
                                }

                                result.Add(studentAndLection);
                            }
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

            catch (NullReferenceException e)
            {
                _iLogger.Log($"Exception :" + e);
                return null;
            }
        }

        public bool DeleteStudentAndLection(int studentId, int lectionId)
        {
            try
            {
                var result = false;
                const string sqlQuery =
                    "DELETE FROM StudentAndLections where studentId = @student and lection = @lection";

                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@studentId", studentId);
                        command.Parameters.AddWithValue("@lectionId", lectionId);

                        var rowsDeletedCount = command.ExecuteNonQuery();
                        if (rowsDeletedCount != 0)
                        {
                            result = true;
                        }
                    }
                }

                return result;
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

                return false;
            }

            catch (ArgumentNullException e)
            {
                _iLogger.Log($"Exception {e}: Your studentId, lectionId can not be null");
                throw;
            }
        }
    }
}