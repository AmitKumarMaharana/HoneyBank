using Microsoft.AspNetCore.Mvc;
using HoneyBank.Models;
using HoneyBank.Data; // Assuming you have a data context class

namespace HoneyBank.Controllers
{
    public class BankDetailsController : Controller
    {
        private readonly HoneyBankDBContext _context;

        public BankDetailsController(HoneyBankDBContext context)
        {
            _context = context;
        }

        // GET: BankDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BankDetails/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BankDetailsModel bankDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bankDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Success)); // Redirect to a success page
            }
            return View(bankDetails);
        }

        // GET: BankDetails/Success
        public IActionResult Success()
        {
            return View();
        }
    }
}
