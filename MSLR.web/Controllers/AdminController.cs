using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSLR.web.Models;

namespace MSLR.web.Controllers
{
    public class AdminController : Controller
    {
        private readonly MslrDbContext _context;

        public AdminController(MslrDbContext context)
        {
            _context = context;
        }
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }
        // Get: Admin/Create
        public IActionResult Create()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            return View();
        }
        // POST: Admin/Create
        [HttpPost]
        public IActionResult Create(string title, string description)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            if (string.IsNullOrWhiteSpace(title))
            {
                ViewBag.Error = "Title is required";
                return View();
            }

            var referendum = new Referendum
            {
                Title = title,
                Description = description,
                Status = "closed"
            };

            _context.Referendums.Add(referendum);
            _context.SaveChanges();
            TempData["Success"] = "Referendum created successfully.";
            return RedirectToAction("Dashboard");

            
        }
        // GET: Admin/Options/5
        public IActionResult Options(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var referendum = _context.Referendums
                .Include(r => r.ReferendumOptions)
                .FirstOrDefault(r => r.ReferendumId == id);

            if (referendum == null)
                return NotFound();

            return View(referendum);
        }
        [HttpPost]
        public IActionResult AddOption(int referendumId, string optionText)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var referendum = _context.Referendums.Find(referendumId);

            if (referendum == null || referendum.Status == "open")
                return BadRequest("Cannot modify options after opening.");

            _context.ReferendumOptions.Add(new ReferendumOption
            {
                ReferendumId = referendumId,
                OptionText = optionText
            });

            _context.SaveChanges();
            return RedirectToAction("Options", new { id = referendumId });
        }
        public IActionResult ToggleStatus(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var referendum = _context.Referendums.Find(id);
            if (referendum == null)
                return NotFound();

            referendum.Status = referendum.Status == "open" ? "closed" : "open";
            _context.SaveChanges();
            TempData["Success"] = "Referendum status updated successfully.";
            return RedirectToAction("Dashboard");

           
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Results()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var results = _context.Referendums
                .Select(r => new ViewModels.ResultViewModel
                {
                    ReferendumId = r.ReferendumId,
                    Title = r.Title,
                    TotalVotes = _context.Votes.Count(v => v.ReferendumId == r.ReferendumId)
                })
                .ToList();

            return View(results);
        }
        public IActionResult Dashboard()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            ViewBag.TotalVoters = _context.Voters
     .Count(v => v.Email != "ec@referendum.gov.sr");

            ViewBag.TotalVotes = _context.Votes.Count();
            ViewBag.TotalReferendums = _context.Referendums.Count();

            var referendums = _context.Referendums.ToList();
            return View(referendums);
        }

        public IActionResult Winner(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            //total voters
            int totalVoters = _context.Voters
      .Count(v => v.Email != "ec@referendum.gov.sr");


            //group votes by option
            var results = _context.Votes
                .Where(v => v.ReferendumId == id)
                .GroupBy(v => v.OptionId)
                .Select(g => new
                {
                    OptionId = g.Key,
                    VoteCount = g.Count()
                })
                .OrderByDescending(x => x.VoteCount)
                .ToList();

            if (!results.Any())
            {
                ViewBag.Message = "No votes have been cast yet.";
                return View();
            }

            //Winner = highest votes
            var winner = results.First();

            //Winner Rules/ Calculation
            bool majorityReached = winner.VoteCount >= (totalVoters / 2.0);

            var option = _context.ReferendumOptions
                .First(o => o.OptionId == winner.OptionId);

            ViewBag.Option = option.OptionText;
            ViewBag.Votes = winner.VoteCount;
            ViewBag.Majority = majorityReached;
            TempData["Success"] = "Winning option calculated successfully.";

            return View();
        }


    }
}
