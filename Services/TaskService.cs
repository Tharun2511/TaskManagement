using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Models;
namespace TaskManagement.Services
{
    public class TaskService: ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _context.Tasks.Where(t => !t.IsDeleted).ToListAsync();
        }
        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }
        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }
        public async Task<TaskItem?> GetByTitle(string title)
        {
            return await _context.Tasks.FindAsync(title);
        }
        public async Task<TaskItem?> UpdateAsync(TaskItem task)
        {
            var existing = await _context.Tasks.FindAsync(task.Id);
            if (existing == null)
                return null;

            existing.Title = task.Title;
            existing.Description = task.Description;

            await _context.SaveChangesAsync();
            return existing;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return false;

            task.IsDeleted = true;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<TaskItem>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.Tasks
                .Where(t => !t.IsDeleted)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<IEnumerable<TaskItem>> SearchAsync(string? status, string? title)
        {
            var query = _context.Tasks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(t =>
                    t.Status != null &&
                    t.Status.ToLower() == status.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(t =>
                    t.Title != null &&
                    t.Title.Contains(title));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetSortedAsync(
    string sortBy,
    bool desc)
        {
            var query = _context.Tasks
                .Where(t => !t.IsDeleted);

            query = sortBy.ToLower() switch
            {
                "title" => desc ? query.OrderByDescending(t => t.Title)
                                : query.OrderBy(t => t.Title),

                "status" => desc ? query.OrderByDescending(t => t.Status)
                                 : query.OrderBy(t => t.Status),

                _ => desc ? query.OrderByDescending(t => t.CreatedAt)
                          : query.OrderBy(t => t.CreatedAt)
            };

            return await query.ToListAsync();
        }


    }
}
