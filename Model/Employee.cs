using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI.Model
{
    public class Employee
    {
        [Key]
        public int Emp_id { get; set; }
        public string Emp_name { get; set; }
        public decimal Emp_salary { get; set; }
        public string Emp_city { get; set;}
        public bool activeFlag { get; set; }
    }
}
