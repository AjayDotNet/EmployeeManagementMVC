using EmployeeManagementMVC.Models;
using EmployeeManagementMVC.Repositories;

namespace EmployeeManagementMVC.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _repository.GetAll();
        }

        public Employee? GetEmployeeById(int id)
        {
            return _repository.GetById(id);
        }

        public void AddEmployee(Employee employee)
        {
            _repository.Add(employee);
            _repository.Save();
        }

        public void UpdateEmployee(Employee employee)
        {
            _repository.Update(employee);
            _repository.Save();
        }

        public void DeleteEmployee(int id)
        {
            _repository.Delete(id);
            _repository.Save();
        }
    }
}