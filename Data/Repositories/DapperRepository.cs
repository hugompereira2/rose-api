using Dapper;
using Microsoft.Data.SqlClient;
using System;

public class DapperRepository
{
    private readonly string _connectionString;

    public DapperRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public bool TestConnection()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            try
            {
                connection.Open();
                connection.Execute("SELECT 1");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
