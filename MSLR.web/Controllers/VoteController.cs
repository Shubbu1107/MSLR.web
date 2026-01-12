using Microsoft.AspNetCore.Mvc;
using MSLR.web.Models;

namespace MSLR.web.Controllers
{
    public class VoteController : Controller
    {
        private readonly MslrDbContext _context;

        public VoteController(MslrDbContext context)
        {
            _context = context;
        }


        //REFERENDUMS LIST 
        public IActionResult Index()
        {
            var voterId = HttpContext.Session.GetInt32("VoterId");

            if (voterId == null ||
                HttpContext.Session.GetString("SCCValidated") != "true")
            {
                return RedirectToAction("Login", "Auth");
            }

            var referendums = _context.Referendums
                .Where(r => r.Status == "open")
                .ToList();

            var votedReferendums = _context.Votes
                .Where(v => v.VoterId == voterId)
                .Select(v => v.ReferendumId)
                .ToList();

            ViewBag.Voted = votedReferendums;

            return View(referendums);
        }

        //VOTING OPTIONS
      
        public IActionResult Vote(int referendumId)
        {
            var voterId = HttpContext.Session.GetInt32("VoterId");
            if (voterId == null)
                return RedirectToAction("Login", "Auth");

            var referendum = _context.Referendums
                .FirstOrDefault(r => r.ReferendumId == referendumId);

            //BLOCK IF CLOSED
            if (referendum == null || referendum.Status == "closed")
            {
                return View("AlreadyVoted");
            }

            bool alreadyVoted = _context.Votes.Any(v =>
                v.VoterId == voterId &&
                v.ReferendumId == referendumId);

            if (alreadyVoted)
            {
                return View("AlreadyVoted");
            }

            var options = _context.ReferendumOptions
                .Where(o => o.ReferendumId == referendumId)
                .ToList();

            ViewBag.ReferendumTitle = referendum.Title;

            return View(options);
        }

  
        // SUBMIT VOTE
       
        [HttpPost]
        public IActionResult Vote(int referendumId, int optionId)
        {
            var voterId = HttpContext.Session.GetInt32("VoterId");
            if (voterId == null)
                return RedirectToAction("Login", "Auth");

            var referendum = _context.Referendums
                .FirstOrDefault(r => r.ReferendumId == referendumId);

            //FINAL SAFETY CHECK
            if (referendum == null || referendum.Status == "closed")
            {
                return View("AlreadyVoted");
            }

            bool alreadyVoted = _context.Votes.Any(v =>
                v.VoterId == voterId &&
                v.ReferendumId == referendumId);

            if (alreadyVoted)
            {
                return View("AlreadyVoted");
            }

            //SAVE VOTE
            var vote = new Vote
            {
                VoterId = voterId.Value,
                ReferendumId = referendumId,
                OptionId = optionId
            };

            _context.Votes.Add(vote);
            _context.SaveChanges();
            
            //AUTO-CLOSE AT 50%
        
            int totalVoters = _context.Voters.Count();

            int votesForOption = _context.Votes.Count(v =>
                v.ReferendumId == referendumId &&
                v.OptionId == optionId);

            int requiredVotes = (int)Math.Ceiling(totalVoters * 0.5);

            if (votesForOption >= requiredVotes)
            {
                referendum.Status = "closed";
                _context.SaveChanges();
            }

            return RedirectToAction("Success");
        }

        // SUCCESS PAGE
        public IActionResult Success()
        {
            if (HttpContext.Session.GetInt32("VoterId") == null)
                return RedirectToAction("Login", "Auth");

            return View();
        }
    }
}
