using Microsoft.AspNetCore.Mvc;
using MSLR.web.Models;

namespace MSLR.web.Controllers.Api
{
    [ApiController]
    [Route("mslr")]
    public class MslrApiController : ControllerBase
    {
        private readonly MslrDbContext _context;

        public MslrApiController(MslrDbContext context)
        {
            _context = context;
        }

        
        //Get  referendums by status
        
        [HttpGet("referendums")]
        public IActionResult GetReferendumsByStatus([FromQuery] string status)
        {
            var referendums = _context.Referendums
                .Where(r => r.Status == status)
                .Select(r => new
                {
                    referendum_id = r.ReferendumId.ToString(),
                    status = r.Status,
                    referendum_title = r.Title,
                    referendum_desc = r.Description,
                    referendum_options = new
                    {
                        options = r.ReferendumOptions.Select(o => new
                        {
                            option_id = o.OptionId.ToString(),
                            option_text = o.OptionText,
                            votes = _context.Votes.Count(v => v.OptionId == o.OptionId).ToString()
                        })
                    }
                })
                .ToList();

            return Ok(new
            {
                Referendums = referendums
            });
        }

      
        //Get referendum by ID
      
        [HttpGet("referendum/{id}")]
        public IActionResult GetReferendumById(int id)
        {
            var referendum = _context.Referendums
                .Where(r => r.ReferendumId == id)
                .Select(r => new
                {
                    referendum_id = r.ReferendumId.ToString(),
                    status = r.Status,
                    referendum_title = r.Title,
                    referendum_desc = r.Description,
                    referendum_options = new
                    {
                        options = r.ReferendumOptions.Select(o => new
                        {
                            option_id = o.OptionId.ToString(),
                            option_text = o.OptionText,
                            votes = _context.Votes.Count(v => v.OptionId == o.OptionId).ToString()
                        })
                    }
                })
                .FirstOrDefault();

            if (referendum == null)
                return NotFound();

            return Ok(referendum);
        }
    }
}