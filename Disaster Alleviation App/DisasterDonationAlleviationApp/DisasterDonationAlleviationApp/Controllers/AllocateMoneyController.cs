using Microsoft.AspNetCore.Mvc;
using DisasterDonationAlleviationApp.Models; 
using DisasterDonationAlleviationApp.Data;   
using System.Linq;

namespace DisasterDonationAlleviationApp.Controllers
{
    public class AllocateMoneyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AllocateMoneyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AllocateMoney
        public IActionResult Index()
        {
            // Get the list of active disasters
            var activeDisasters = _context.Disasters.Where(d => d.EndDate == null || d.EndDate > DateTime.Now).ToList();

            // Pass active disasters to the view
            return View(activeDisasters);
        }

        // POST: AllocateMoney
        [HttpPost]
        public IActionResult Allocate(int disasterId, decimal amount)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError(string.Empty, "Amount must be greater than zero.");
                return RedirectToAction(nameof(Index));
            }

            // Get the disaster by ID
            var disaster = _context.Disasters.Find(disasterId);
            if (disaster == null)
            {
                ModelState.AddModelError(string.Empty, "Disaster not found.");
                return RedirectToAction(nameof(Index));
            }

            // Check if there are enough funds available
            var totalMonetaryDonations = _context.MonetaryDonations.Sum(d => d.Amount);
            var totalAllocated = _context.AllocatedFunds.Sum(a => a.Amount);
            var availableFunds = totalMonetaryDonations - totalAllocated;

            if (amount > availableFunds)
            {
                ModelState.AddModelError(string.Empty, "Insufficient funds available.");
                return RedirectToAction(nameof(Index));
            }

            // Allocate the money to the disaster
            var allocation = new AllocatedFund
            {
                DisasterId = disasterId,
                Amount = amount,
                AllocationDate = DateTime.Now
            };

            _context.AllocatedFunds.Add(allocation);
            _context.SaveChanges();

            TempData["SuccessMessage"] = $"Successfully allocated {amount:C} to {disaster.Location}.";
            return RedirectToAction(nameof(Index));
        }
    }
}
