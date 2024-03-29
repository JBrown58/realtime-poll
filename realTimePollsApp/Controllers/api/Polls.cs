﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using realTimePolls.Models;

namespace realTimePolls.Controllers
{
    public class Polls : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly RealTimePollsContext _context; // Declare the DbContext variable

        private readonly IWebHostEnvironment _env;

        private readonly IConfiguration _configuration;

        public Polls(
            ILogger<HomeController> logger,
            RealTimePollsContext context,
            IWebHostEnvironment env,
            IConfiguration configuration
        ) // Inject DbContext in the constructor
        {
            _logger = logger;
            _context = context; // Initialize the _context variable. This the DbContext instance.
            _env = env;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var polls = _context
                    .Polls.Include(p => p.Genre)
                    .Select(p => new PollItem
                    {
                        Poll = p,
                        FirstVoteCount = _context
                            .UserPoll.Where(up => up.PollId == p.Id && up.Vote == true)
                            .Count(),
                        SecondVoteCount = _context
                            .UserPoll.Where(up => up.PollId == p.Id && up.Vote == false)
                            .Count(),
                        UserName = _context.User.SingleOrDefault(user => user.Id == p.UserId).Name,
                        ProfilePicture = _context
                            .User.SingleOrDefault(user => user.Id == p.UserId)
                            .ProfilePicture
                    })
                    .ToList();

                int pollCount = _context.Polls.Count();

                var pollList = new PollsList { Polls = polls, PollCount = pollCount };

                return Json(pollList);
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }

        [HttpGet]
        public IActionResult GetDropdownList()
        {
            try
            {
                var dropdownList = _context.Genre.OrderBy(g => g.Name).ToList();

                var options = new { options = dropdownList };

                return Json(options);
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }

        [HttpGet]
        public IActionResult GetSearchResults([FromQuery] string search)
        {
            try
            {
                var pattern = $"%{search}%";

                var polls = _context
                    .Polls.Include(p => p.Genre)
                    .Where(c => EF.Functions.Like(c.Title.ToLower(), pattern.ToLower()))
                    .Select(p => new PollItem
                    {
                        Poll = p,
                        FirstVoteCount = _context
                            .UserPoll.Where(up => up.PollId == p.Id && up.Vote == true)
                            .Count(),
                        SecondVoteCount = _context
                            .UserPoll.Where(up => up.PollId == p.Id && up.Vote == false)
                            .Count(),
                        UserName = _context.User.SingleOrDefault(user => user.Id == p.UserId).Name,
                        ProfilePicture = _context
                            .User.SingleOrDefault(user => user.Id == p.UserId)
                            .ProfilePicture
                    })
                    .ToList();

                int pollLength = _context
                    .Polls.Where(c => EF.Functions.Like(c.Title, pattern))
                    .Select(p => new PollItem { Poll = p, })
                    .Count();

                var pollList = new PollsList { Polls = polls, PollCount = pollLength };

                return Json(pollList);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Json(null);
            }
        }

        [HttpPost]
        public IActionResult GetGenreResults([FromBody] int genreId)
        {
            try
            {
                var polls = _context
                    .Polls.Include(p => p.Genre)
                    .Where(p => p.GenreId == genreId)
                    .Select(p => new PollItem
                    {
                        Poll = p,
                        FirstVoteCount = _context
                            .UserPoll.Where(up => up.PollId == p.Id && up.Vote == true)
                            .Count(),
                        SecondVoteCount = _context
                            .UserPoll.Where(up => up.PollId == p.Id && up.Vote == false)
                            .Count(),
                        UserName = _context.User.SingleOrDefault(user => user.Id == p.UserId).Name,
                        ProfilePicture = _context
                            .User.SingleOrDefault(user => user.Id == p.UserId)
                            .ProfilePicture
                    })
                    .ToList();

                int pollLength = _context
                    .Polls.Where(p => p.GenreId == genreId)
                    .Select(p => new PollItem { Poll = p, })
                    .Count();

                var pollList = new PollsList { Polls = polls, PollCount = pollLength };

                return Json(pollList);
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }
            );
        }
    }
}
