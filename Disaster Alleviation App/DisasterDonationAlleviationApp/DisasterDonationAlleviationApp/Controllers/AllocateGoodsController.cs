using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DisasterDonationAlleviationApp.Models;
using DisasterDonationAlleviationApp.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DisasterDonationAlleviationApp.Controllers
{
    public class AllocateGoodsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AllocateGoodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AllocateGoods
        public async Task<IActionResult> Index()
        {
            var model = new AllocateGoodsViewModel
            {
                Disasters = await _context.Disasters.ToListAsync(),
                GoodsDonations = await _context.GoodsDonations.ToListAsync()
            };

            return View(model);
        }

        // POST: AllocateGoods
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Allocate(AllocateGoodsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var disaster = await _context.Disasters.FindAsync(model.SelectedDisasterId);
                var goods = await _context.GoodsDonations.FindAsync(model.SelectedGoodsId);

                if (disaster != null && goods != null)
                {
                    var allocation = new DisasterGoodsAllocation
                    {
                        DisasterId = disaster.Id,
                        GoodsDonationId = goods.Id,
                        Quantity = model.Quantity
                    };

                    _context.DisasterGoodsAllocations.Add(allocation);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Invalid disaster or goods donation selected.");
            }

            return View("Index", new AllocateGoodsViewModel
            {
                Disasters = await _context.Disasters.ToListAsync(),
                GoodsDonations = await _context.GoodsDonations.ToListAsync()
            });
        }
    }
}
