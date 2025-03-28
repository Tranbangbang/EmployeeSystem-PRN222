using System.ComponentModel.DataAnnotations;

namespace EmployManager.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên vai trò")]
        public string? Name { get; set; }

        public ICollection<Employee>? Employees { get; set; }
    }
}
