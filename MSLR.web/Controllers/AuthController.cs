using Microsoft.AspNetCore.Mvc;
using MSLR.web.Models;
using MSLR.web.Helpers;
using MSLR.web.ViewModels;

namespace MSLR.web.Controllers
{
    public class AuthController : Controller
    {
        private readonly MslrDbContext _context;

        public AuthController(MslrDbContext context)
        {
            _context = context;
        }

        //Get: /Auth/Register

        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetInt32("VoterId") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //POST:/Auth/Register
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            //Age validation    
            var today = DateTime.Today;
            var dob = model.DOB.ToDateTime(TimeOnly.MinValue);

            int age = today.Year - dob.Year;
            if (dob > today.AddYears(-age))
            {
                age--;
            }

            if (age < 18)
            {
                ViewBag.Error = "You must be at least 18 years old to register and vote.";
                return View(model);
            }


            //SCC validation
            var scc = _context.ValidSccs.FirstOrDefault(s => s.Scc == model.SCC);
            if (scc == null)
            {
                ViewBag.Error = "Invalid SCC code.";
                return View(model);
            }

            if (scc.IsUsed)
            {
                ViewBag.Error = "This SCC code has already been used.";
                return View(model);
            }

            //Email uniqueness
            if (_context.Voters.Any(v => v.Email == model.Email))
            {
                ViewBag.Error = "Email is already registered.";
                return View(model);
            }

            var voter = new Voter
            {
                FullName = model.FullName,
                Email = model.Email,
                DateOfBirth = model.DOB,
                PasswordHash = HashHelper.Hash(model.Password),
                Scc = model.SCC
            };

            _context.Voters.Add(voter);
            scc.IsUsed = true;

            _context.SaveChanges();

            TempData["Success"] = "Registration successful. You can now log in.";
            return RedirectToAction("Login");

        }

        //Get:/Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("VoterId") != null)
            {
                var role = HttpContext.Session.GetString("Role");

                if (role == "Admin")
                    return RedirectToAction("Dashboard", "Admin");

                return RedirectToAction("Index", "Vote");
            }

            return View();
        }


        //POST/:/Auth/Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var voter = _context.Voters.FirstOrDefault(v => v.Email == model.Email);

            //Email not registered
            if (voter == null)
            {
                ModelState.AddModelError("", "Account not found. Please register first.");
                return View(model);
            }

            //Password incorrect
            var hashedInput = HashHelper.Hash(model.Password);
            if (hashedInput != voter.PasswordHash)
            {
                ModelState.AddModelError("", "Incorrect password.");
                return View(model);
            }

            //SUCCESSFUL LOGIN
            HttpContext.Session.SetInt32("VoterId", voter.VoterId);
            HttpContext.Session.SetString("UserName", voter.FullName);

            if (voter.Email == "ec@referendum.gov.sr")
            {
                HttpContext.Session.SetString("Role", "Admin");
                return RedirectToAction("Dashboard", "Admin");
            }

            HttpContext.Session.SetString("Role", "Voter");
          
            return RedirectToAction("ValidateCode");
        }
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("VoterId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public IActionResult ValidateCode()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ValidateCode(string scc)
        {
            var voterId = HttpContext.Session.GetInt32("VoterId");
            if (voterId == null)
                return RedirectToAction("Login");

            var voter = _context.Voters.Find(voterId);

            if (voter.Scc != scc)
            {
                ModelState.AddModelError("", "Invalid SCC code");
                return View();
            }

            HttpContext.Session.SetString("SCCValidated", "true");
            return RedirectToAction("Index", "Vote");
        }
    }
}
