using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DisasterDonationAlleviationApp.Models;
using System.Threading.Tasks;
using DisasterDonationAlleviationApp.Data;

namespace DisasterDonationAlleviationApp.Controllers
{
    [Authorize]
    public class MonetaryDonationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MonetaryDonationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MonetaryDonations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MonetaryDonations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,Amount,IsAnonymous")] MonetaryDonation monetaryDonation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(monetaryDonation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect to the list of donations
            }
            return View(monetaryDonation);
        }

        // GET: MonetaryDonations/Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.MonetaryDonations.ToListAsync());
        }
    }
}
