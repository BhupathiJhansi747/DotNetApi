using EmployeeAPI.Abstraction;
using EmployeeAPI.DatabaseContext;
using EmployeeAPI.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Repository
{
    public class EmployeeRepository : IEmployee
    {
        private readonly AppDbContext _context;
        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<Employee> GetEmployees()
        {
            var sp = "[dbo].[GetEmployee]";
            try
            {
                return _context.Employee.FromSqlRaw(sp).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<Employee> GetEmployeeById(int empId)
        {
            var sp = "[dbo].[GetEmployeeById] @empId";

            try
            {
                var resultData = _context.Employee.FromSqlRaw(sp,
                    new SqlParameter("@empId", empId)).AsEnumerable();

                return resultData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void AddEmployee(Employee employee)
        {
            var sp = "[dbo].[AddEmployee] @empId, @empName, @empSalary, @empCity, @activeFlag";

            try
            {
                var parameters = new SqlParameter[]
                {
                new SqlParameter("@empId", employee.Emp_id),
                new SqlParameter("@empName", employee.Emp_name),
                new SqlParameter("@empSalary", employee.Emp_salary),
                new SqlParameter("@empCity", employee.Emp_city),
                new SqlParameter("@activeFlag", employee.activeFlag)
                };

                _context.Database.ExecuteSqlRaw(sp, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateEmployee(int empId, Employee employee)
        {
            var sp = "[dbo].[UpdateEmployee] @empId, @empName, @empSalary, @empCity, @activeFlag";
            var existingEmployee = _context.Employee.Find(empId);
            try
            {
                if (existingEmployee.Emp_id == empId)
                {
                    var parameters = new SqlParameter[]
              {
                new SqlParameter("@empId", employee.Emp_id),
                new SqlParameter("@empName", employee.Emp_name),
                new SqlParameter("@empSalary", employee.Emp_salary),
                new SqlParameter("@empCity", employee.Emp_city),
                new SqlParameter("@activeFlag", employee.activeFlag)
              };

                    _context.Database.ExecuteSqlRaw(sp, parameters);
                    _context.SaveChanges();
                }
                  
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteEmployee(int empId)
        {
            var sp = "[dbo].[DeleteEmployee] @empId";

            try
            {
                var parameter = new SqlParameter("@empId", empId);

                _context.Database.ExecuteSqlRaw(sp, parameter);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
