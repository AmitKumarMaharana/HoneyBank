using HoneyBank.Data;
using HoneyBank.Models;
using Microsoft.AspNetCore.Mvc;

namespace HoneyBank.Controllers
{
    public class LoginController : Controller
    {
        private readonly HoneyBankDBContext _context;

        public LoginController(HoneyBankDBContext context)
        {
            _context = context;
        }

        public IActionResult LoginPage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginUser(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.User
                    .Where(u => u.Username == model.Username)
                    .Where(u => u.Password == model.Password)
                    .FirstOrDefault();
                if (model.Username != null && model.Password != user.Password)
                { 
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return View("LoginPage", model);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View("LoginPage", model);
            }
            // Redirect to SignUpSuccess with context = "Login"
            return RedirectToAction("SignUpSuccess", new { context = "Login" });
        }
        public IActionResult SignUpSuccess(string context)
        {
            if (context == "Login")
            {
                ViewData["SuccessTitle"] = "Login Successful";
                ViewData["SuccessMessage"] = "Welcome back! You have successfully logged in.";
                ViewData["SuccessContext"] = "Login";
            }

            return View("~/Views/Dashboard/SignUpSuccess.cshtml");
        }
    }
}
