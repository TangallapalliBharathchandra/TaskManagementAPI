

using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> AddTask(TaskManagementAPI.Models.Task task)
        {
            await _taskService.AddTaskAsync(task);
            return Ok("Task added successfully.");
        }

        [HttpPut("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> UpdateTask(int id, TaskManagementAPI.Models.Task task)
        {
            task.Id = id;
            await _taskService.UpdateTaskAsync(task);
            return Ok("Task updated successfully.");
        }
    }
}
