using System.ComponentModel.DataAnnotations;
namespace TaskManagement.DTO
{
    public class TaskCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int AssignedToUserId { get; set; }
    }
}
