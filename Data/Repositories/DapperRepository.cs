using Dapper;
using Microsoft.Data.SqlClient;
using rose_api.Models;

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
                var logRequisitions = connection.Query<LogError>("SELECT * from log_error");
                foreach (var logRequisition in logRequisitions)
                {
                    Console.WriteLine($"ID: {logRequisition.Id}, EndPoint: {logRequisition.EndPoint}, ...");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public void InsertLogRequisition(LogRequisition logRequisition)
    {
        using var connection = new SqlConnection(_connectionString);

        try
        {
            connection.Open();
            string sql = "INSERT INTO log_requisition (endPoint, parameters, data, ip, createdDate) " +
                         "VALUES (@EndPoint, @Parameters, @Data, @Ip, @CreatedDate)";

            connection.Execute(sql, logRequisition);
        }
        catch (Exception ex)
        {
            LogError logError = new LogError
            {
                EndPoint = logRequisition.EndPoint,
                Parameters = logRequisition.Parameters,
                Error = $"Erro em InsertLogRequisition: {ex.Message}",
                Ip = logRequisition.Ip,
                CreatedDate = DateTime.Now
            };

            InsertLogError(logError);
        }
    }

    public void InsertLogError(LogError LogError)
    {
        using var connection = new SqlConnection(_connectionString);

        try
        {
            connection.Open();
            string sql = "INSERT INTO log_error (endPoint, parameters, error, ip, createdDate) " +
                         "VALUES (@EndPoint, @Parameters, @Error, @Ip, @CreatedDate)";

            connection.Execute(sql, LogError);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao inserir log: {ex.Message}");
        }
    }

    public void InsertErrorLog(string endPoint, string parameters, string error, string ip)
    {
        LogError logError = new LogError
        {
            EndPoint = endPoint,
            Parameters = parameters,
            Error = error,
            Ip = ip,
            CreatedDate = DateTime.Now
        };

        InsertLogError(logError);
    }

    public void InsertErrorLog(string endPoint, string parameters, Exception ex, string ip)
    {
        InsertErrorLog(endPoint, parameters, ex.Message, ip);
    }
}
