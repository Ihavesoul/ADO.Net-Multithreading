using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using module_20.Helpers;
using module_20.Models;

namespace module_20.DataAccessLayer
{
    public class LectorDal
    {
        private readonly IConfig _iConfig;
        private readonly ILogger _iLogger;

        public LectorDal(ILogger iLogger, IConfig iConfig)
        {
            _iLogger = iLogger;
            _iConfig = iConfig;
        }

        public LectorDal()
        {
        }

        public void InsertLector(string firstName, string secondName, string email)
        {
            try
            {
                const string sqlQuery = "INSERT INTO Lectors (firstName,secondName,email) Values (@firstName,@secondName,@email)";
                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@firstName", firstName);
                        command.Parameters.AddWithValue("@secondName", secondName);
                        command.Parameters.AddWithValue("@email", email);

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
                _iLogger.Log($"Exception {e}: Your firstName,secondName,email can not be null");
                throw;
            }
        }

        public void UpdateLector(int lectorId, string firstName, string secondName, string email)
        {
            try
            {
                const string updateQuery = "UPDATE Lectors SET firstName = @firstName,secondName = @secondName, email = @email Where lectorId = @lectorId";

                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@firstName", firstName);
                        command.Parameters.AddWithValue("@secondName", secondName);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@lector", lectorId);
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
                _iLogger.Log($"Exception {e}: Your firstName,secondName,email can not be null");
                throw;
            }
        }

        public Lector GetLectorById(int lectorId)
        {
            try
            {
                var lector = new Lector();

                const string sqlQuery = "SELECT firstName,secondName from Lectors where lectorId=@lectorId";

                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@lector", lectorId);

                        var dataReader = command.ExecuteReader();

                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (Int32.TryParse(dataReader["lectorId"]?.ToString(), out lector.lectorId.id))
                                {
                                    throw new FormatException(nameof(lector.lectorId));
                                }

                                lector.firstName = dataReader["firstName"]?.ToString();
                                lector.secondName = dataReader["secondName"]?.ToString();
                                lector.email = dataReader["email"]?.ToString();
                            }
                        }
                    }
                }
                return lector;
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
                _iLogger.Log($"Exception {e}: Your lectorId can not be null");
                throw;
            }
        }

        public List<Lector> GetLectors()
        {
            try
            {
                var result = new List<Lector>();
                
                const string sqlQuery = "SELECT firstName,secondName from Lectors";

                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        var dataReader = command.ExecuteReader();

                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                var lector = new Lector();

                                if (Int32.TryParse(dataReader["lectorId"].ToString(), out lector.lectorId.id))
                                {
                                    throw new FormatException(nameof(lector.lectorId));
                                }

                                lector.firstName = dataReader["firstName"]?.ToString();
                                lector.secondName = dataReader["secondName"]?.ToString();
                                lector.email = dataReader["email"]?.ToString();
                                result.Add(lector);
                            }
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

                return null;
            }
        }

        public bool DeleteLector(int lectorId)
        {
            try
            {
                var result = false;
                const string sqlQuery = "DELETE FROM Lectors where lector = @lector";

                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@lector", lectorId);

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
                    _iLogger.Log("Returned : false");
                }

                return false;
            }

            catch (ArgumentNullException e)
            {
                _iLogger.Log($"Exception {e}: Your lectorId can not be null");
                throw;
            }
        }
    }
}