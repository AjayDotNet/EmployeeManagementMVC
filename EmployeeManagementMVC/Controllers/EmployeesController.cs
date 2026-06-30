using EmployeeManagementMVC.Models;
using EmployeeManagementMVC.Services;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace EmployeeManagementMVC.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _service;

        public EmployeesController(IEmployeeService service)
        {
            _service = service;
        }

        // GET: Employees
           public IActionResult Index(string sortOrder, string search, string department, int page = 1)
        {
            int pageSize = 5;

            var employees = _service.GetAllEmployees();

            // =========================
            // DASHBOARD STATS
            // =========================
            ViewBag.TotalEmployees = employees.Count();
            ViewBag.AvgSalary = employees.Any() ? employees.Average(e => e.Salary) : 0;
            ViewBag.DepartmentCount = employees.Select(e => e.Department).Distinct().Count();

            // SEARCH
            if (!string.IsNullOrEmpty(search))
            {
                employees = employees
                    .Where(e => e.Name.ToLower().Contains(search.ToLower()))
                    .ToList();
            }

            // FILTER
            if (!string.IsNullOrEmpty(department))
            {
                employees = employees
                    .Where(e => e.Department == department)
                    .ToList();
            }

            // =========================
            // SORT VARIABLES
            // =========================
            ViewBag.NameSort = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.SalarySort = sortOrder == "salary_asc" ? "salary_desc" : "salary_asc";
            ViewBag.DepartmentSort = sortOrder == "dept_asc" ? "dept_desc" : "dept_asc";

            // SORTING
            employees = sortOrder switch
            {
                "name_desc" => employees.OrderByDescending(e => e.Name).ToList(),
                "name_asc" => employees.OrderBy(e => e.Name).ToList(),

                "salary_desc" => employees.OrderByDescending(e => e.Salary).ToList(),
                "salary_asc" => employees.OrderBy(e => e.Salary).ToList(),

                "dept_desc" => employees.OrderByDescending(e => e.Department).ToList(),
                "dept_asc" => employees.OrderBy(e => e.Department).ToList(),

                _ => employees.OrderBy(e => e.Name).ToList()
            };

            var result = employees.ToPagedList(page, pageSize);

            return View(result);
        }

        // DETAILS
        public IActionResult Details(int? id)
        {
            if (id == null) return RedirectToAction("Index");

            var employee = _service.GetEmployeeById(id.Value);

            if (employee == null)
                return RedirectToAction("Index");

            return View(employee);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _service.AddEmployee(employee);
                TempData["Success"] = "Employee added successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(employee);
        }

        // EDIT (GET)
        public IActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("Index");

            var employee = _service.GetEmployeeById(id.Value);

            if (employee == null)
                return RedirectToAction("Index");

            return View(employee);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                TempData["Error"] = "Invalid employee data!";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                _service.UpdateEmployee(employee);
                TempData["Success"] = "Employee updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(employee);
        }

        // DELETE (GET)
        public IActionResult Delete(int? id)
        {
            if (id == null) return RedirectToAction("Index");

            var emp = _service.GetEmployeeById(id.Value);

            if (emp == null)
                return RedirectToAction("Index");

            return View(emp);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var emp = _service.GetEmployeeById(id);

            if (emp == null)
            {
                TempData["Error"] = "Employee not found!";
                return RedirectToAction("Index");
            }

            _service.DeleteEmployee(id);

            TempData["Success"] = "Employee deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}