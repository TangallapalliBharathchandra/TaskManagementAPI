using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace TaskManagementAPI.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)] // Only Date, No Time in Swagger UI
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public string Priority { get; set; } // High, Medium, Low
        public string Status { get; set; }  // New, In Progress, Finished
    }
}