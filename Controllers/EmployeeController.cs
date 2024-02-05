using EmployeeAPI.Abstraction;
using EmployeeAPI.Helper;
using EmployeeAPI.Model;
using EmployeeAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _employeeRepository;
        public EmployeeController(IEmployee employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        [HttpGet]
        public ActionResult GetEmployee()
        {
            try
            {
                var employees = _employeeRepository.GetEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message);
            }

        }
        [HttpGet]
        public ActionResult GetEmployeeById(int id)
        {
            var employee = _employeeRepository.GetEmployeeById(id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }
        [HttpPost]
        public ActionResult AddEmployee(Employee employee)



        {
            _employeeRepository.AddEmployee(employee);
            return Ok(employee);
        }
        [HttpPut("{id}")]
        public ActionResult UpdateEmployee(int id, Employee employee)
        {
            var existingEmployee = _employeeRepository.GetEmployeeById(id);
            if (existingEmployee == null)
                return NotFound();

            employee.Emp_id = id;
            _employeeRepository.UpdateEmployee(id, employee);

            return Ok("Successfully Updated");
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteEmployee(int id)
        {
            var existingEmployee = _employeeRepository.GetEmployeeById(id);
            if (existingEmployee == null)
                return NotFound();

            _employeeRepository.DeleteEmployee(id);

            return Ok("Successfully Deleted");
        }

        [HttpGet]
        public async Task<ActionResult> GetDAO(int? Id)
        {
            BaseHttpClient _client = new BaseHttpClient("http://localhost:7073/api/");
            var data = await _client.Get<IList<Villa>>($"getVilla?Id={Id}");
            return Ok(data);
        }
    }
}
