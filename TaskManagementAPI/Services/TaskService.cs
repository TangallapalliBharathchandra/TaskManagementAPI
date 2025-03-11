using System.Data;
using System.Data.SqlClient;
using TaskManagementAPI.Models;
using TaskManagementAPI.Infrastructure;

namespace TaskManagementAPI.Services
{
    public class TaskService
    {
        private readonly TaskRepository _repository;

        public TaskService(TaskRepository repository)
        {
            _repository = repository;
        }

        public async System.Threading.Tasks.Task AddTaskAsync(TaskManagementAPI.Models.Task task)
        {
            ValidateTask(task);
            await ValidateHighPriorityTasksAsync(task);
            await _repository.AddTaskAsync(task);
        }

        public async System.Threading.Tasks.Task UpdateTaskAsync(TaskManagementAPI.Models.Task task)
        {
            ValidateTask(task);
            await ValidateHighPriorityTasksAsync(task);
            await _repository.UpdateTaskAsync(task);
        }

        private void ValidateTask(TaskManagementAPI.Models.Task task)
        {
            if (task.DueDate < DateTime.Now)
                throw new ArgumentException("Due date cannot be in the past.");

            if (task.DueDate.DayOfWeek == DayOfWeek.Saturday || task.DueDate.DayOfWeek == DayOfWeek.Sunday)
                throw new ArgumentException("Due date cannot be on a weekend.");

            if (IsHoliday(task.DueDate))
                throw new ArgumentException("Due date cannot be on a holiday.");
        }

      


        private async System.Threading.Tasks.Task ValidateHighPriorityTasksAsync(TaskManagementAPI.Models.Task task)
        {
            if (task.Priority.Equals("High", StringComparison.OrdinalIgnoreCase))  // Ensure case-insensitive comparison
            {
                int count = await _repository.GetHighPriorityTaskCountAsync(task.DueDate);

                // Debugging step
                Console.WriteLine($"High Priority Task Count on {task.DueDate}: {count}");

                if (count > 100)
                    throw new ArgumentException($"Cannot have more than 100 high-priority unfinished tasks on {task.DueDate}. Current count: {count}");
            }
        }


        private static bool IsHoliday(DateTime date)
        {
            var holidays = GetHolidays(date.Year);
            return holidays.Contains(date.Date);
        }

        private static List<DateTime> GetHolidays(int year)
        {
            return new List<DateTime>
    {
        new DateTime(year, 1, 1),  // New Year's Day
        GetNthWeekdayOfMonth(year, 1, DayOfWeek.Monday, 3), // Martin Luther King Jr. Day (3rd Monday of January)
        GetNthWeekdayOfMonth(year, 2, DayOfWeek.Monday, 3), // Presidents' Day (3rd Monday of February)
        new DateTime(year, 5, 27), // Memorial Day (Last Monday of May)
        new DateTime(year, 6, 19), // Juneteenth National Independence Day
        GetNthWeekdayOfMonth(year, 7, DayOfWeek.Thursday, 4), // Independence Day (July 4th, stays static)
        GetNthWeekdayOfMonth(year, 9, DayOfWeek.Monday, 1), // Labor Day (1st Monday of September)
        GetNthWeekdayOfMonth(year, 10, DayOfWeek.Monday, 2), // Columbus Day (2nd Monday of October)
        new DateTime(year, 11, 11), // Veterans Day (November 11)
        GetNthWeekdayOfMonth(year, 11, DayOfWeek.Thursday, 4), // Thanksgiving Day (4th Thursday of November)
        new DateTime(year, 12, 25) // Christmas Day
    };
        }

        private static DateTime GetNthWeekdayOfMonth(int year, int month, DayOfWeek dayOfWeek, int nth)
        {
            if (nth < 1 || nth > 5)
                throw new ArgumentException("Nth value must be between 1 and 5.");

            DateTime date = new DateTime(year, month, 1);
            int count = 0;

            while (date.Month == month)
            {
                if (date.DayOfWeek == dayOfWeek)
                {
                    count++;
                    if (count == nth)
                        return date;
                }
                date = date.AddDays(1);
            }

            throw new ArgumentException($"Could not find the {nth} {dayOfWeek} in {month}/{year}");
        }

    }
}