using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagement.DTO;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _service;
        private readonly ILogger<TasksController> _logger;

        public TasksController(
            ITaskService service,
            ILogger<TasksController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all records");

            var tasks = await _service.GetAllAsync();

            var result = tasks.Select(t => new TaskReadDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                CreatedByUserId = t.CreatedByUserId,
                AssignedToUserId = t.AssignedToUserId,
                CreatedAt = t.CreatedAt
            });

            return Ok(result);
        }

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _service.GetByIdAsync(id);

            if (task == null)
            {
                _logger.LogWarning("Task not found with id {Id}", id);
                return NotFound();
            }

            var dto = new TaskReadDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                CreatedByUserId = task.CreatedByUserId,
                AssignedToUserId = task.AssignedToUserId,
                CreatedAt = task.CreatedAt
            };

            return Ok(dto);
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<IActionResult> Create(TaskCreateDto dto)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = "Pending",
                CreatedByUserId = userId,
                AssignedToUserId = dto.AssignedToUserId,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _service.CreateAsync(task);

            return Ok(new TaskReadDto
            {
                Id = created.Id,
                Title = created.Title,
                Description = created.Description,
                Status = created.Status,
                CreatedByUserId = created.CreatedByUserId,
                AssignedToUserId = created.AssignedToUserId,
                CreatedAt = created.CreatedAt
            });
        }

        // GET: api/tasks/by/title/{title}
        [HttpGet("by/title/{title}")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            var tasks = await _service.GetByTitle(title);
            return Ok(tasks);
        }

        // DELETE: api/tasks/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        // PUT: api/tasks/{id}
        [Authorize(Policy = "TaskOwnerOnly")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskUpdateDto dto)
        {
            if (id != dto.Id)
            {
                _logger.LogWarning(
                    "ID mismatch. Route id: {Id}, DTO id: {DtoId}",
                    id, dto.Id);
                return BadRequest("ID mismatch");
            }

            var task = new TaskItem
            {
                Id = dto.Id,
                Title = dto.Title,
                Status = dto.Status
            };

            var updated = await _service.UpdateAsync(task);

            if (updated == null)
                return NotFound();

            return Ok(updated);
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search(
      [FromQuery] string? status,
      [FromQuery] string? title)
        {
            _logger.LogInformation(
                "Searching tasks. Status={Status}, Title={Title}",
                status, title);

            var tasks = await _service.SearchAsync(status, title);

            var result = tasks.Select(t => new TaskReadDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                CreatedByUserId = t.CreatedByUserId,
                AssignedToUserId = t.AssignedToUserId,
                CreatedAt = t.CreatedAt
            });

            return Ok(result);
        }


        [HttpGet("sorted")]
        public async Task<IActionResult> GetSorted(
    string sortBy = "createdAt",
    bool desc = true)
        {
            var result = await _service.GetSortedAsync(sortBy, desc);
            return Ok(result);
        }

    }
}
