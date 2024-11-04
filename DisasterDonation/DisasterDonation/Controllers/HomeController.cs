using DisasterDonation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DisasterDonation.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DisasterDonationDbContext _context;

        public HomeController(ILogger<HomeController> logger, DisasterDonationDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        private decimal GetAvailableFunds()
        {
            // Calculate total funds from monetary donations, excluding any negative values (deductions)
            var totalFunds = _context.MonetaryDonations
                .Where(d => d.Amount > 0 && d.AllocatedDisasterId == null)
                .Sum(d => d.Amount);

            // Subtract any deductions or expenses
            var totalDeductions = _context.MonetaryDonations
                .Where(d => d.Amount < 0 && d.AllocatedDisasterId == null)
                .Sum(d => d.Amount);

            return totalFunds + totalDeductions;
        }

        [Authorize]
        public IActionResult PurchaseGoods()
        {
            var viewModel = new PurchaseGoodsViewModel
            {
                Disasters = _context.Disasters.Where(d => d.IsActive).ToList(),
                AvailableGoods = new List<PredefinedGood>
            {
                new PredefinedGood {
                    Id = 1,
                    Category = "Food",
                    Description = "Emergency Food Kit",
                    UnitCost = 50.00m,
                    AvailableQuantity = 100
                },
                new PredefinedGood {
                    Id = 2,
                    Category = "Medical",
                    Description = "First Aid Kit",
                    UnitCost = 30.00m,
                    AvailableQuantity = 200
                },
                // Add more predefined goods as needed
            },
                AvailableFunds = GetAvailableFunds()
            };
            return View(viewModel);
        }

        private List<AidType> GetPredefinedAidTypes()
        {
            return new List<AidType>
    {
        new AidType { Id = 1, Name = "Food", Description = "Food supplies for disaster relief." },
        new AidType { Id = 2, Name = "Medical", Description = "Medical supplies for health care." },
        new AidType { Id = 3, Name = "Shelter", Description = "Shelter facilities and supplies." },
        // Add more aid types as needed
    };
        }

        [Authorize]
        [HttpPost]
        public IActionResult PurchaseGoods(PurchaseGoodsViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Re-fetch active disasters and available goods
            model.Disasters = _context.Disasters.Where(d => d.IsActive).ToList();
            model.AvailableGoods = new List<PredefinedGood>
    {
        new PredefinedGood {
            Id = 1,
            Category = "Food",
            Description = "Emergency Food Kit",
            UnitCost = 50.00m,
            AvailableQuantity = 100
        },
        new PredefinedGood {
            Id = 2,
            Category = "Medical",
            Description = "First Aid Kit",
            UnitCost = 30.00m,
            AvailableQuantity = 200
        },
        // Add more predefined goods as needed
    };
            model.AvailableFunds = GetAvailableFunds();

            var availableFunds = model.AvailableFunds;
            decimal totalCost = 0;

            // Calculate total cost of selected goods
            foreach (var item in model.SelectedGoods.Where(sg => sg.Quantity > 0))
            {
                var predefinedGood = model.AvailableGoods.FirstOrDefault(ag => ag.Id == item.GoodsId);
                if (predefinedGood != null)
                {
                    totalCost += predefinedGood.UnitCost * item.Quantity;
                }
            }

            // Check if user has enough funds
            if (totalCost > availableFunds)
            {
                ModelState.AddModelError("", "Insufficient funds for this purchase.");
                return View(model);
            }

            try
            {
                // Create GoodsDonation entries for each purchased item
                foreach (var item in model.SelectedGoods.Where(sg => sg.Quantity > 0))
                {
                    var predefinedGood = model.AvailableGoods.FirstOrDefault(ag => ag.Id == item.GoodsId);
                    if (predefinedGood != null)
                    {
                        var goodsDonation = new GoodsDonation
                        {
                            Date = DateTime.Now,
                            NumberOfItems = item.Quantity,
                            Cost = predefinedGood.UnitCost * item.Quantity,
                            Category = predefinedGood.Category,
                            Description = predefinedGood.Description,
                            AllocatedDisasterId = model.SelectedDisasterId
                        };
                        _context.GoodsDonations.Add(goodsDonation);
                    }
                }

                // Create a negative monetary donation to reflect the spent funds
                var monetaryDeduction = new MonetaryDonation
                {
                    Date = DateTime.Now,
                    Amount = -totalCost,
                    IsAnonymous = true,
                    AllocatedDisasterId = model.SelectedDisasterId
                };
                _context.MonetaryDonations.Add(monetaryDeduction);

                _context.SaveChanges();

                return RedirectToAction("GoodsDonations");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while processing your purchase.");
                return View(model);
            }
        }



        public IActionResult PublicInfo()
        {
            // Calculate total monetary donations
            var totalMonetaryDonations = _context.MonetaryDonations.Sum(x => x.Amount);

            // Calculate total number of goods received
            var totalGoodsDonations = _context.GoodsDonations.Sum(x => x.NumberOfItems);

            // Get active disasters
            var activeDisasters = _context.Disasters
                .Where(d => d.IsActive)
                .ToList();

            // Create the view model
            var viewModel = new PublicInfoViewModel
            {
                ActiveDisasters = activeDisasters
            };

            // Pass the totals using ViewBag
            ViewBag.MonetaryDonations = totalMonetaryDonations;
            ViewBag.GoodsDonations = totalGoodsDonations;

            return View(viewModel);
        }
        
        [Authorize]
        public IActionResult AllocateMoney()
        {
            var viewModel = new AllocateMoneyViewModel
            {
                ActiveDisasters = _context.Disasters.Where(d => d.IsActive).ToList() ?? new List<Disaster>(),
                UnallocatedMonetaryDonations = _context.MonetaryDonations.Where(m => m.AllocatedDisasterId == null).ToList() ?? new List<MonetaryDonation>()
            };

            // Log or check the counts to ensure data is present
            Debug.WriteLine($"ActiveDisasters Count: {viewModel.ActiveDisasters?.Count() ?? 0}");
            Debug.WriteLine($"UnallocatedMonetaryDonations Count: {viewModel.UnallocatedMonetaryDonations?.Count() ?? 0}");

            return View(viewModel);
        }
        [Authorize]

        [HttpPost]
        public IActionResult AllocateMoney(AllocateMoneyViewModel model)
        {
            var monetaryDonation = _context.MonetaryDonations.SingleOrDefault(g => g.Id == model.MonetaryDonationId);
            if (monetaryDonation != null)
            {
                monetaryDonation.AllocatedDisasterId = model.DisasterId;
                _context.SaveChanges();
            }
            return RedirectToAction("MonetaryDonations");
        }
        [Authorize]
        public IActionResult AllocateGoods()
        {
            var viewModel = new AllocateGoodsViewModel
            {
                ActiveDisasters = _context.Disasters.Where(d => d.IsActive).ToList(),
                UnallocatedGoodsDonations = _context.GoodsDonations.Where(g => g.AllocatedDisasterId == null).ToList()
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AllocateGoods(AllocateGoodsViewModel model)
        {
            var goodsDonation = _context.GoodsDonations.SingleOrDefault(g => g.Id == model.GoodsDonationId);
            if (goodsDonation != null)
            {
                goodsDonation.AllocatedDisasterId = model.DisasterId;
                _context.SaveChanges();
            }
            return RedirectToAction("GoodsDonations");
        }

        [Authorize]
        public IActionResult CreateEditMonetaryDonationForm(MonetaryDonation model)
        {
            if (model.Id == 0)
            {
                _context.MonetaryDonations.Add(model);

            }
            else
            {
                _context.MonetaryDonations.Update(model);
            }

            _context.SaveChanges();
            return RedirectToAction("MonetaryDonations");
        }

        public IActionResult CreateEditMonetaryDonation(int? id)
        {
            if (id != null)
            {
                var monetaryDonationInDb = _context.MonetaryDonations.SingleOrDefault(x => x.Id == id);
                return View(monetaryDonationInDb);
            }

            return View();
        }


        [Authorize]
        public IActionResult MonetaryDonations()
        {
            var allMonetaryDonations = _context.MonetaryDonations.ToList();
            var totalMonetaryDonations = allMonetaryDonations.Sum(x => x.Amount);
            ViewBag.MonetaryDonations = totalMonetaryDonations;
            return View(allMonetaryDonations);
            
        }
        [Authorize]
        public IActionResult GoodsDonations()
        {
            var allGoodsDonations = _context.GoodsDonations.ToList();
            var totalGoodsDonations = allGoodsDonations.Sum(x => x.NumberOfItems);
            ViewBag.GoodsDonations = totalGoodsDonations;
            return View(allGoodsDonations);

        }
        [Authorize]
        public IActionResult Disasters()
        {
            var allDisasters = _context.Disasters.ToList();
            ViewBag.AidTypes = GetPredefinedAidTypes(); // Use predefined aid types
            return View(allDisasters);
        }
        [Authorize]
        public IActionResult CreateEditDisasterForm(Disaster model)
        {
            if (model.Id == 0)
            {
                _context.Disasters.Add(model);
            }
            else
            {
                _context.Disasters.Update(model);
            }

            _context.SaveChanges();
            return RedirectToAction("Disasters");

        }

        public IActionResult CreateEditDisaster(int? id)
        {
            // Populate Aid Types
            ViewBag.AidTypes = GetPredefinedAidTypes();

            if (id != null)
            {
                var disasterInDb = _context.Disasters.SingleOrDefault(x => x.Id == id);
                return View(disasterInDb);
            }

            return View(new Disaster());
        }


        [Authorize]
        public IActionResult DeleteDisaster(int id)
        {
            var disasterInDb = _context.Disasters.SingleOrDefault(x => x.Id == id);
            _context.Disasters.Remove(disasterInDb);
            _context.SaveChanges();
            return RedirectToAction("Disasters");

        }
        [Authorize]
        public IActionResult CreateEditGoodsDonationForm(GoodsDonation model)
        {
            if (model.Id == 0)
            {
                _context.GoodsDonations.Add(model);

            }
            else
            {
                _context.GoodsDonations.Update(model);
            }

            _context.SaveChanges();
            return RedirectToAction("GoodsDonations");
        }

        public IActionResult CreateEditGoodsDonation(int? id)
        {
            if (id != null)
            {
                var goodsDonationInDb = _context.GoodsDonations.SingleOrDefault(x => x.Id == id);
                return View(goodsDonationInDb);
            }

            return View();
        }
        [Authorize]
        public IActionResult DeleteGoodsDonation(int id)
        {
            var goodsDonationInDb = _context.GoodsDonations.SingleOrDefault(x => x.Id == id);
            _context.GoodsDonations.Remove(goodsDonationInDb);
            _context.SaveChanges();
            return RedirectToAction("GoodsDonations");

        }
        [Authorize]
        public IActionResult DeleteMonetaryDonation(int id)
        {
            var monetaryDonationInDb = _context.MonetaryDonations.SingleOrDefault(x => x.Id == id);
            _context.MonetaryDonations.Remove(monetaryDonationInDb);
            _context.SaveChanges();
            return RedirectToAction("MonetaryDonations");

        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
