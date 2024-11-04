using Microsoft.AspNetCore.Mvc;
using DisasterDonationAlleviationApp.Data;
using DisasterDonationAlleviationApp.Models;
using System.Linq;

namespace DisasterDonationAlleviationApp.Controllers
{
    public class PublicInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublicInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Total monetary donations
            var totalDonations = _context.MonetaryDonations.Sum(d => d.Amount);

            // Total number of goods
            var totalGoods = _context.GoodsDonations.Sum(d => d.NumberOfItems);

            // Active disasters
            var activeDisasters = _context.Disasters
                .Where(d => d.EndDate >= DateTime.UtcNow)
                .ToList();

            var viewModel = new PublicInfoViewModel
            {
                TotalDonations = totalDonations,
                TotalGoods = totalGoods,
                ActiveDisasters = activeDisasters
            };

            return View(viewModel);
        }
    }
}
