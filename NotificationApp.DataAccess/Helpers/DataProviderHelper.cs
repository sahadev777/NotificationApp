using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace NotificationApp.DataAccess.Helpers
{
    internal class DataProviderHelper
    {
        internal async Task<T> StoredProcAsync<T>(string connectionString, string storedProc, Func<DbDataReader, Task<T>> readerParserAction, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(storedProc, connection))
                {
                    try
                    {
                        command.CommandTimeout = 900;
                        command.CommandType = CommandType.StoredProcedure;
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            return await readerParserAction(reader);
                        }
                    }
                    finally
                    {
                        command.Parameters.Clear();
                    }
                }
            }
        }

        internal async Task<object> StoredProcScalerAsync(string connectionString, string storedProc, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                await connection.OpenAsync();

                using (var command = new SqlCommand(storedProc, connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                        return await command.ExecuteScalarAsync();
                    }
                    finally
                    {
                        command.Parameters.Clear();
                    }
                }
            }
        }

        internal async Task StoredProcNonQueryAsync(string connectionString, string storedProc, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                await connection.OpenAsync();

                using (var command = new SqlCommand(storedProc, connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }

                        await command.ExecuteNonQueryAsync();
                    }
                    finally
                    {
                        command.Parameters.Clear();
                    }
                }
            }
        }
    }
}
