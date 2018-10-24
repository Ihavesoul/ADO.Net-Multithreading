using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using module_20.BusinessLogic.Helpers;
using module_20.Helpers;
using module_20.Models;

namespace module_20.DataAccessLayer
{
    public class LectionDal
    {
        private readonly IConfig _iConfig;
        private readonly ILogger _iLogger;

        public LectionDal(ILogger iLogger, IConfig iConfig)
        {
            _iLogger = iLogger;
            _iConfig = iConfig;
        }

        public LectionDal()
        {
        }

        public void InserLection(int lectorId, string lectionName, DateTime lectionData)
        {
            try
            {
                const string sqlQuery =
                    "INSERT INTO lection (lector,LectionName,LectionData) Values (@lector,@LectionName,@LectionData)";
                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@lectorId", lectorId);
                        command.Parameters.AddWithValue("@LectionName", lectionName);
                        command.Parameters.AddWithValue("@LectionData", lectionData);
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
                _iLogger.Log($"Exception {e}: Your lectorId,lectionName,lectionData can not be null");
                throw;
            }
        }

        public void UpdateLection(int lectorId, string lectionName, DateTime lectionData)
        {
            try
            {
                const string updateQuery =
                    "UPDATE lection SET lector = @lector,LectionName = @LectionName,LectionData = @LectionData where lector = @lector";

                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@lectorId", lectorId);
                        command.Parameters.AddWithValue("@LectionName", lectionName);
                        command.Parameters.AddWithValue("@LectionData", lectionData);

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
                _iLogger.Log($"Exception {e}: Your lectorId,lectionName,lectionData can not be null");
                throw;
            }
        }

        public Lection GetLectionById(int? lectionId)
        {
            try
            {
                const string sqlQuery = "SELECT L.LectionName,L.LectionData," +
                                        "LRS.FirstName,LRS.SecondName,LRS.LectorId " +
                                        "From Lection " +
                                        "where L.LectionId = @lectionId" +
                                        "INNER JOIN Lection L ON L.LectionId = L.LectionId " +
                                        "INNER JOIN Lectors LRS ON L.LectorId  = LRS.LectorId";

                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@lectionId", lectionId);

                        var dataReader = command.ExecuteReader();

                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                var lection = new Lection(dataReader.GetLector());
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
                }

                return null;
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
                _iLogger.Log($"Exception {e}: Your LectionID can not be null");
                throw;
            }
            
        }

        public List<Lection> GetLection()
        {
            try
            {
                var result = new List<Lection>();
                
                const string sqlQuery = "SELECT L.LectionName,L.LectionData," +
                                        "LRS.FirstName,LRS.SecondName,LRS.LectorId " +
                                        "From Lection " + 
                                        "INNER JOIN Lection L ON L.LectionId = L.LectionId " +
                                        "INNER JOIN Lectors LRS ON L.LectorId  = LRS.LectorId";

                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        var dataReader = command.ExecuteReader();

                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                var lection = new Lection(dataReader.GetLector());

                                if (!int.TryParse(dataReader["LectionId"]?.ToString(), out lection.lectionId.id))
                                {
                                    throw new FormatException($"{lection.lectionId} can not be parse"+nameof(lection.lectionId));
                                }

                                if (!int.TryParse(dataReader["LectorId"]?.ToString(), out lection.lector.lectorId.id))
                                {
                                    throw new FormatException($"{lection.lectionId.id} can not be pars" + nameof(lection.lector));
                                }

                                lection.name = dataReader["LectionName"]?.ToString();
                                if (!DateTime.TryParse(dataReader["LectionData"].ToString(), out lection.date))
                                {
                                    throw new FormatException($"{lection.date} can not be pars" + nameof(lection.date));
                                }

                                result.Add(lection);
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

        public bool DeleteLection(int lectionId)
        {
            try
            {
                var result = false;
                const string sqlQuery = "DELETE FROM lection where lection = @lection";

                using (var connection = new SqlConnection(_iConfig.DbConnectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
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
                    _iLogger.Log("Returned : False");
                }
                return false;
            }
            catch (ArgumentNullException e)
            {
                _iLogger.Log($"Exception {e}: Your LectionID can not be null");
                throw;
            }
        }
    }
}