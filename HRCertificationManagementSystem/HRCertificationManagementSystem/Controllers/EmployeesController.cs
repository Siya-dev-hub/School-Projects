
using HRCertificationManagementSystem.Data;
using HRCertificationManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace HRCertificationManagementSystem.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<EmployeesController> _logger;

    

    public EmployeesController(
        ApplicationDbContext context,
        IWebHostEnvironment webHostEnvironment,
        ILogger<EmployeesController> logger)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
        
    }

    // GET: /Employees

    [HttpGet]
    public async Task<IActionResult> Index(string search, string sortOrder)
    {
        var employees = from e in _context.Employees select e;

        if (!string.IsNullOrEmpty(search))
        {
            employees = employees.Where(e => e.FirstName.Contains(search) || e.LastName.Contains(search));
        }

        // Sorting logic
        employees = sortOrder switch
        {
            "date_asc" => employees.OrderBy(e => e.CertificationExpirationDate),
            "date_desc" => employees.OrderByDescending(e => e.CertificationExpirationDate),
            _ => employees
        };

        return View(await employees.ToListAsync());
    }


    // GET: /Employees/Create
    [Route("Create")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Employees/Create
    [Route("Create")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FirstName,LastName,Certification,CertificationExpirationDate,Email,PhoneNumber,CertificationFile")] Employee employee)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Handle PDF upload if provided
                if (employee.CertificationFile != null && employee.CertificationFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "certifications");
                    Directory.CreateDirectory(uploadsFolder);
                    var fileName = $"{Guid.NewGuid()}_{employee.CertificationFile.FileName}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await employee.CertificationFile.CopyToAsync(stream);
                    }
                    employee.CertificationPdfPath = "/certifications/" + fileName;
                }

                _context.Add(employee);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating employee: {ex.Message}");
                ModelState.AddModelError("", "Unable to save the employee. Please try again.");
            }
        }

        return View(employee);
    }

    // GET: Employees/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return View(employee);
    }

    // POST: Employees/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Certification,CertificationExpirationDate,Email,PhoneNumber")] Employee employee, IFormFile? certificationPdf)
    {
        if (id != employee.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Update existing employee without requiring re-upload of PDF
                var existingEmployee = await _context.Employees.FindAsync(id);
                if (existingEmployee == null)
                {
                    return NotFound();
                }

                // Update properties
                existingEmployee.FirstName = employee.FirstName;
                existingEmployee.LastName = employee.LastName;
                existingEmployee.Certification = employee.Certification;
                existingEmployee.CertificationExpirationDate = employee.CertificationExpirationDate;
                existingEmployee.Email = employee.Email;
                existingEmployee.PhoneNumber = employee.PhoneNumber;

                // Handle PDF upload if a new file is provided
                if (certificationPdf != null && certificationPdf.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "certifications");
                    Directory.CreateDirectory(uploadsFolder); // Ensure folder exists
                    var filePath = Path.Combine(uploadsFolder, certificationPdf.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await certificationPdf.CopyToAsync(stream);
                    }
                    existingEmployee.CertificationPdfPath = "/certifications/" + certificationPdf.FileName;
                }

                // Save changes
                _context.Update(existingEmployee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(employee.Id))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(employee);
    }


    // GET: Employees/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _context.Employees
            .FirstOrDefaultAsync(m => m.Id == id);
        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }

    // POST: Employees/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EmployeeExists(int id)
    {
        return _context.Employees.Any(e => e.Id == id);
    }
}
}

