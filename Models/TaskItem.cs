public class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "Pending";

    // Assignment
    public int CreatedByUserId { get; set; }
    public int? AssignedToUserId { get; set; }

    // Auditing
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedByUserId { get; set; }

    // Soft delete
    public bool IsDeleted { get; set; } = false;
}
