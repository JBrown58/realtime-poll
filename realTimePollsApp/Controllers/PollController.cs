﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using realTimePolls.Models;

namespace realTimePolls.Controllers
{
    public class PollController : Controller
    {
        private readonly ILogger<PollController> _logger;

        private readonly RealTimePollsContext _context; // Declare the DbContext variable

        public PollController(ILogger<PollController> logger, RealTimePollsContext context)
        {
            _logger = logger;
            _context = context;
        }

        public class JsonRequestItem
        {
            public string jsonRequest { get; set; }
        }

        [HttpGet]
        public IActionResult Index([FromQuery] string polltitle, int pollid, int userid)
        {
            Poll poll = _context.Polls.FirstOrDefault(u =>
                u.Id == pollid && u.UserId == userid && u.Title == polltitle
            );

            if (poll == null)
                throw new Exception("Poll cannot be found");

            int firstVoteCount = _context.UserPoll.Count(up =>
                    up.PollId == poll.Id && up.Vote == true
                ),
                secondVoteCount = _context.UserPoll.Count(up =>
                    up.PollId == poll.Id && up.Vote == true
                );

            UserPoll userPoll =
                _context.UserPoll.FirstOrDefault(up => up.UserId == userid && up.PollId == pollid)
                ?? null;

            var viewModel = _context
                .Polls.Select(p => new PollItem
                {
                    Poll = poll,
                    FirstVoteCount = firstVoteCount,
                    SecondVoteCount = secondVoteCount,
                    Vote = userPoll == null ? false : userPoll.Vote,
                })
                .FirstOrDefault();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(
            [FromForm] string title,
            [FromForm] string firstOption,
            [FromForm] string secondOption
        )
        {
            try
            {
                var googleId =
                    HttpContext != null ? HttpContext.User.Claims.ToList()[0].Value : string.Empty;

                if (googleId == string.Empty)
                    throw new Exception("Could not find googleId");

                int userId = _context.User.SingleOrDefault(u => u.GoogleId == googleId).Id;

                Poll poll = new Poll
                {
                    UserId = userId,
                    Title = title,
                    FirstOption = firstOption,
                    SecondOption = secondOption
                };

                _context.Polls.Add(poll);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home", new { area = "" });
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }

        [HttpPost]
        public ActionResult Delete(string pollid)
        {
            try
            {
                var pollId = Int32.Parse(pollid);
                var poll = _context.Polls.FirstOrDefault(poll => poll.Id == pollId);
                if (poll == null)
                {
                    throw new Exception("Could not find pollId");
                }
                _context.Polls.Remove(poll);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }

        [HttpPost]
        public ActionResult Vote([FromForm] int userid, int pollid, string vote)
        {
            try
            {
                bool userVoter;

                if (vote == "Vote First")
                    userVoter = true;
                else if (vote == "Vote Second")
                    userVoter = false;
                else
                    throw new Exception("Unable to vote");

                var userPoll = _context.UserPoll.FirstOrDefault(userPoll =>
                    userPoll.PollId == pollid && userPoll.UserId == userid
                );

                UserPoll UserPoll = new UserPoll()
                {
                    UserId = userid,
                    PollId = pollid,
                    Vote = userVoter
                };

                if (userPoll == null)
                    _context.UserPoll.Add(UserPoll);
                else
                    userPoll.Vote = userVoter;

                _context.SaveChanges();

                return RedirectToAction("Index", "Home", new { area = "" });
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }
    }
}
