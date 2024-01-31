﻿using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using realTimePolls.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        // GET: PollController
        public ActionResult Index(string data, string pollid, string userid)
        {
            var pollId = Int32.Parse(pollid);
            var userId = Int32.Parse(userid);
            var polls = _context.Polls.ToList();

            var poll = polls.FirstOrDefault(u => u.Title == data);
            var pollTitles = polls.ConvertAll<string>(poll => poll.Title);
            var userPolls = _context.UserPoll.ToList();
            var userPoll = userPolls.FirstOrDefault(userPoll =>
                userPoll.Poll == pollId && userPoll.UserId == userId
            );

            var viewModel = new PollsViewModel
            {
                Poll = poll,
                PollTitles = pollTitles,
                UserPoll = userPoll
            };
            return View(viewModel);
        }

        // GET: PollController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Vote(string vote, string pollid, string userid)
        {
            var pollId = Int32.Parse(pollid);
            var userId = Int32.Parse(userid);

            var polls = _context.Polls.ToList();
            var users = _context.User.ToList();
            var userPolls = _context.UserPoll.ToList();

            var poll = polls.FirstOrDefault(u => u.Id == pollId); //get the current poll
            var user = users.FirstOrDefault(user => user.Id == userId); //get the current user
            var userPoll = userPolls.FirstOrDefault(userPoll =>
                userPoll.Poll == pollId && userPoll.UserId == userId
            ); // get the current userPoll
            var pollTitles = polls.ConvertAll<string>(poll => poll.Title);
            // fix return model
            PollsViewModel viewModel = new PollsViewModel
            {
                Polls = polls,
                PollTitles = pollTitles,
                FirstOption = poll.FirstOption,
                SecondOption = poll.SecondOption,
            };

            var UserPoll = new UserPoll
            {
                UserId = userId,
                Poll = poll.Id,
                FirstVote = vote == "Vote First" ? true : false,
                SecondVote = vote == "Vote Second" ? true : false
            };

            if (userPoll == null)
            {
                _context.UserPoll.Add(UserPoll);
            }
            else
            {
                userPoll.FirstVote = UserPoll.FirstVote;
                userPoll.SecondVote = UserPoll.SecondVote;
            }

            _context.SaveChanges();
            return View("../Home/Index", viewModel);
        }
    }
}
