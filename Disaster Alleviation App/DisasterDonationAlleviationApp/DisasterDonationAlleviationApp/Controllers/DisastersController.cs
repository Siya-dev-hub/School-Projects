using DisasterDonationAlleviationApp.Data;
using DisasterDonationAlleviationApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace DisasterDonationAlleviationApp.Controllers
{
    public class DisastersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DisastersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Disasters
        public IActionResult Index()
        {
            try
            {
                // Check if the database connection is successful
                var canConnect = _context.Database.CanConnect();
                if (canConnect)
                {
                    Console.WriteLine("Database connection successful.");
                }
                else
                {
                    Console.WriteLine("Failed to connect to the database.");
                }

                // Retrieve disasters and related data
                var disasters = _context.Disasters
                    .Include(d => d.DisasterAidTypes)
                    .ThenInclude(dat => dat.AidType)
                    .ToList();

                return View(disasters);
            }
            catch (Exception ex)
            {
                // Log the exception if there's an issue connecting or retrieving data
                Console.WriteLine($"Error: {ex.Message}");
                return View("Error");
            }
        }

        // GET: Disasters/Create
        public IActionResult Create()
        {
            var viewModel = new DisasterCreateViewModel
            {
                AidTypes = _context.AidTypes.ToList() // Fetch aid types from the database
            };
            return View(viewModel);
        }

        // POST: Disasters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DisasterCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var disaster = viewModel.Disaster;

                // Initialize the collection if it is null
                disaster.DisasterAidTypes = disaster.DisasterAidTypes ?? new List<DisasterAidType>();

                // Add selected aid types to the disaster
                foreach (var aidTypeId in viewModel.SelectedAidTypes)
                {
                    disaster.DisasterAidTypes.Add(new DisasterAidType { AidTypeId = aidTypeId });
                }

                _context.Disasters.Add(disaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If model validation fails, repopulate the AidTypes list and return to the view
            viewModel.AidTypes = _context.AidTypes.ToList();
            return View(viewModel);
        }
    }
}
