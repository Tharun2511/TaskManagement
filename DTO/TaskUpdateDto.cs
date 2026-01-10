using System.ComponentModel.DataAnnotations;

namespace TaskManagement.DTO
{
    public class TaskUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
