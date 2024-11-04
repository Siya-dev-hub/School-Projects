using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DisasterDonationAlleviationApp.Models;
using System.Threading.Tasks;
using DisasterDonationAlleviationApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DisasterDonationAlleviationApp.Controllers
{
    [Authorize]
    public class GoodsDonationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GoodsDonationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GoodsDonations/Index
        public async Task<IActionResult> Index()
        {
            var goodsDonations = await _context.GoodsDonations.ToListAsync();
            return View(goodsDonations);
        }

        // GET: GoodsDonations/Create
        public IActionResult Create()
        {
            // Predefined categories for the dropdown list
            ViewData["Categories"] = new SelectList(new[] { "Clothes", "Non-perishable foods" });
            return View();
        }

        // POST: GoodsDonations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,NumberOfItems,Category,Description,IsAnonymous")] GoodsDonation goodsDonation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(goodsDonation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If something went wrong, reload the categories list
            ViewData["Categories"] = new SelectList(new[] { "Clothes", "Non-perishable foods" });
            return View(goodsDonation);
        }

        // GET: GoodsDonations/ManageCategories
        public IActionResult ManageCategories()
        {
            return View();
        }

        // POST: GoodsDonations/ManageCategories
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageCategories(string newCategory)
        {
            if (!string.IsNullOrEmpty(newCategory))
            {
                // This is a simple example. You should have a dedicated Category model and table for managing categories.
                // For now, we are just simulating adding a category.
                // In a real app, you'd save this to the database and adjust the Create view to load categories dynamically.

                // This example assumes you're handling categories in-memory or using a predefined list.
                // Save category logic should be here if using a database or persistent storage.
            }

            return RedirectToAction(nameof(Create));
        }
    }

}
