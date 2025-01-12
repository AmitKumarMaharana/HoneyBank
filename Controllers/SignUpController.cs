using HoneyBank.Data;
using HoneyBank.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml; // EPPlus namespace

namespace HoneyBank.Controllers
{
    public class SignUpController : Controller
    {
        private readonly HoneyBankDBContext _context;

        public SignUpController(HoneyBankDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SignUpPage()
        {
            return View();
        }

        private readonly string _excelFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", "BankDetails.xlsx");

        // Handle form submission
        [HttpPost]
        public IActionResult SignUpUser(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Save User data
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password
                };

                _context.User.Add(user);
                _context.SaveChanges();

                // Save UserDetails data (using the UserID from the saved user)
                var userDetails = new UserDetails
                {
                    UserID = user.UserID,
                    Name = model.Name,
                    Email = model.Email,
                    Address = model.Address
                };

                _context.UserDetails.Add(userDetails);
                _context.SaveChanges();

                try
                {
                    // Set EPPlus license context
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    // Open the Excel file
                    using var package = new ExcelPackage(new FileInfo(_excelFilePath));
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    int assignedBankDetailsID = -1;
                    String NoDataFound = "0";
                    // Find the first unassigned bank detail in Excel
                    for (int row = 2; row <= rowCount; row++) // Start from row 2 to skip headers
                    {
                        if (string.IsNullOrWhiteSpace(worksheet.Cells[row, 2].Value?.ToString())) // Check if UserID is null
                        {
                            NoDataFound = "1";
                            // Extract BankDetails values from Excel
                            var bankDetails = new BankDetailsModel
                            {
                                AccountNumber = worksheet.Cells[row, 3].Value?.ToString(),
                                IFSCCode = worksheet.Cells[row, 4].Value?.ToString(),
                                AccountCategory = worksheet.Cells[row, 5].Value?.ToString(),
                                OtherInfo = worksheet.Cells[row, 6].Value?.ToString(),
                                UserID = user.UserID // Assign UserID from the saved user
                            };

                            // Insert bank details into the database
                            _context.BankDetails.Add(bankDetails);
                            _context.SaveChanges();

                            // Mark the assigned row in Excel
                            assignedBankDetailsID = bankDetails.BankDetailsID; // Auto-generated ID
                            worksheet.Cells[row, 1].Value = bankDetails.BankDetailsID; // Populate UserID in Excel
                            worksheet.Cells[row, 2].Value = user.UserID; // Populate UserID in Excel
                            break;
                        }
                    }
                    if(NoDataFound == "0")
                    {
                        // Set Default Values if no data is found in the excel
                        var bankDetails = new BankDetailsModel
                        {
                            AccountNumber = "92000000",
                            IFSCCode = "Default",
                            AccountCategory = "Saving",
                            OtherInfo = "This information was created by default as the Excel is not updated",
                            UserID = user.UserID // Assign UserID from the saved user
                        };

                        // Insert bank details into the database
                        _context.BankDetails.Add(bankDetails);
                        _context.SaveChanges();

                        assignedBankDetailsID = bankDetails.BankDetailsID; // Auto-generated ID
                    }

                    if (assignedBankDetailsID == -1)
                    {
                        return BadRequest("No available bank details to assign.");
                    }

                    // Save changes to the Excel file
                    package.Save();

                    // Redirect to success page
                    return View("SignUpSuccess");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            // If invalid, redisplay the form with errors
            return View("SignUpUser", model);
        }

        // Render the success page
        public IActionResult SignUpSuccess()
        {
            return View();
        }
    }
}

