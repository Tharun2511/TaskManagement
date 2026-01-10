using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;

namespace TaskManagement.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(int id);
        Task<TaskItem> CreateAsync(TaskItem task);
        Task<TaskItem?> GetByTitle(string title);
        Task<TaskItem?> UpdateAsync(TaskItem task);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TaskItem>> GetPagedAsync(int page, int pageSize);

        // Filtering
        Task<IEnumerable<TaskItem>> SearchAsync(string? status, string? title);

        // Sorting
        Task<IEnumerable<TaskItem>> GetSortedAsync(string sortBy, bool desc);


    }
}
