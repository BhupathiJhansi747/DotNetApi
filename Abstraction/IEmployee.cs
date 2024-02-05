using EmployeeAPI.Model;

namespace EmployeeAPI.Abstraction
{
    public interface IEmployee
    {
        List<Employee> GetEmployees();
        IEnumerable<Employee> GetEmployeeById(int empId);
        void AddEmployee(Employee employee);
        void UpdateEmployee(int empId, Employee employee);
        void DeleteEmployee(int empId);
    }
}
