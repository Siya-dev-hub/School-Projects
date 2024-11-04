using Microsoft.AspNetCore.Mvc;
using DisasterDonationAlleviationApp.Models;
using DisasterDonationAlleviationApp.Data;

namespace DisasterDonationAlleviationApp.Controllers
{
    public class PurchaseGoodsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseGoodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Create a view model with lists for goods and available money if needed
            var model = new PurchaseGoodsViewModel
            {
                Goods = _context.Goods.ToList(), // Fetch all goods
                Disasters = _context.Disasters.Where(d => d.IsActive).ToList(), // Fetch active disasters
                AvailableMoney = _context.MonetaryDonations.Sum(d => d.Amount) // Calculate total available money
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(PurchaseGoodsViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Implement the purchase logic here
                var totalCost = model.SelectedGoods.Sum(g => g.Cost * g.Quantity);

                // Ensure there is enough available money
                var availableMoney = _context.MonetaryDonations.Sum(d => d.Amount);
                if (totalCost > availableMoney)
                {
                    ModelState.AddModelError("", "Not enough money available to complete this purchase.");
                    return View(model);
                }

                // Process the purchase
                foreach (var item in model.SelectedGoods)
                {
                    _context.Purchases.Add(new Purchase
                    {
                        GoodsId = item.GoodsId,
                        Quantity = item.Quantity,
                        DisasterId = model.SelectedDisasterId
                    });
                }

                // Deduct money
                _context.MonetaryDonations.Add(new MonetaryDonation
                {
                    Amount = -totalCost, // Deduct the cost
                    Date = DateTime.Now
                });

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            // If model state is invalid, return to the same view
            return View(model);
        }
    }
}
