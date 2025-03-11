

using System.Data;
using System.Data.SqlClient;

using Microsoft.Extensions.Configuration;
using TaskManagementAPI.Models;
using System.Threading.Tasks;
using Dapper;

namespace TaskManagementAPI.Infrastructure
{
    public class TaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async System.Threading.Tasks.Task<int> AddTaskAsync(TaskManagementAPI.Models.Task task) // Resolved ambiguity
        {
            using var connection = new SqlConnection(_connectionString); // Inline connection creation
            return await connection.ExecuteAsync("sp_AddTask", new
            {
                task.Name,
                task.Description,
                task.DueDate,
                task.StartDate,
                task.Priority,
                task.Status
            }, commandType: CommandType.StoredProcedure);
        }

        public async System.Threading.Tasks.Task<int> UpdateTaskAsync(TaskManagementAPI.Models.Task task) // Resolved ambiguity
        {
            using var connection = new SqlConnection(_connectionString); // Inline connection creation
            return await connection.ExecuteAsync("sp_UpdateTask", new
            {
                task.Id,
                task.Name,
                task.Description,
                task.DueDate,
                task.StartDate,
                task.EndDate,
                task.Priority,
                task.Status
            }, commandType: CommandType.StoredProcedure);
        }

        public async System.Threading.Tasks.Task<int> GetHighPriorityTaskCountAsync(DateTime dueDate) // Resolved ambiguity
        {
            using var connection = new SqlConnection(_connectionString); // Inline connection creation
            return await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Tasks WHERE DueDate = @DueDate AND Priority = 'High' AND Status != 'Finished'",
                new { DueDate = dueDate });
        }
    }
}
