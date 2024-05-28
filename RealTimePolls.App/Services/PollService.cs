﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;

namespace RealTimePolls.Repositories
{
    public class PollService : IPollService
    {
        private readonly RealTimePollsDbContext dbContext;
        private readonly IHelpersService helpersRepository;
        private readonly IMapper mapper;

        public PollService(RealTimePollsDbContext dbContext, IHelpersService helpersRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.helpersRepository = helpersRepository;
            this.mapper = mapper;
        }

        public async Task<PollViewModel> Index(string pollTitle, int pollId, int userId)
        {
            var poll = await dbContext.Polls
                .Include(p => p.User)
                .Include(p => p.Genre)
                .FirstOrDefaultAsync(p => p.Title == pollTitle && p.Id == pollId && p.UserId == userId);

            if (poll == null)
                return null;

            var userPolls = await dbContext.UserPoll
                .Where(up => up.PollId == poll.Id)
                .ToListAsync();

            int firstVoteCount = userPolls.Count(up => up.Vote == true);
            int secondVoteCount = userPolls.Count(up => up.Vote == false);
            bool? userVote = userPolls.FirstOrDefault(up => up.UserId == userId)?.Vote;

            return new PollViewModel
            {
                Poll = mapper.Map<PollDto>(poll),
                FirstVoteCount = firstVoteCount,
                SecondVoteCount = secondVoteCount,
                Vote = userVote
            };
        }

        public async Task<PollDto> CreatePollAsync(AddPollRequest addPollRequest)
        {
            var domainPoll = mapper.Map<Poll>(addPollRequest);
            await dbContext.Polls.AddAsync(domainPoll);
            await dbContext.SaveChangesAsync();
            return mapper.Map<PollDto>(domainPoll);      
        }

        public async Task DeletePollAsync(int pollId)
        {
            var userPolls = await dbContext.UserPoll.Where(up => up.PollId == pollId).ToListAsync();

            if (userPolls.Any())
            {
                dbContext.UserPoll.RemoveRange(userPolls);
            }

            var poll = dbContext.Polls.SingleOrDefault(p => p.Id == pollId);

            dbContext.Polls.Remove(poll);
            return;
        }

        public async Task<UserPoll> VoteAsync(AuthenticateResult result, AddVoteRequest addVoteRequest)
        {
            bool userVoter;
            var currentUserId = await helpersRepository.GetUserId(result);
            var userPoll = new UserPoll();

            if (addVoteRequest.Vote == "Vote First")
                userVoter = true;
            else if (addVoteRequest.Vote == "Vote Second")
                userVoter = false;
            else
                throw new Exception("Unable to vote");

            var userPolls = await dbContext.UserPoll.ToListAsync();

            var currentUserPoll = userPolls.FirstOrDefault(up =>
                up.PollId == addVoteRequest.PollId && up.UserId == currentUserId);

            if (currentUserPoll == null)
            {
                userPoll.UserId = currentUserId;
                userPoll.PollId = addVoteRequest.PollId;
                userPoll.Vote = userVoter;
                await dbContext.UserPoll.AddAsync(userPoll);
            }
            else
            {
                currentUserPoll.Vote = userVoter;
            }

            await dbContext.SaveChangesAsync();
            await helpersRepository.SendMessage();

            return userPoll;
        }


    }
}
