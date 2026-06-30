using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementMVC.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Department { get; set; }

        [Range(10000, 1000000, ErrorMessage = "Salary must be at least 10000")]
        public decimal Salary { get; set; }
    }
}