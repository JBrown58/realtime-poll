﻿using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using RealTimePolls.Data;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;
using RealTimePolls.Repositories;

namespace RealTimePolls.Repositories
{
    public class SQLHomeRepository : IHomeRepository
    {
        private readonly RealTimePollsDbContext dbContext;

        public SQLHomeRepository(
            RealTimePollsDbContext dbContext
        )
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Genre>> GetDropdownList()
        {
            var dropdownList = await dbContext.Genre.OrderBy(g => g.Name).ToListAsync();

            return dropdownList;
        }

        public Task<IActionResult> GetPolls()
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetUserProfilePicture(AuthenticateResult result)
        {
            if (result.Principal == null)
                return string.Empty;

            var claims = result
                .Principal.Identities.FirstOrDefault()
                ?.Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                })
                .ToList();

            User newUser;
            string? userName = null;
            string? userEmail = null;

            if (claims == null || !claims.Any())
            {
                throw new ArgumentOutOfRangeException("Claims count cannot be 0");
            }

            var googleId = claims
                .FirstOrDefault(c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                )
                .Value;

            string profilePicture = dbContext
                .User.SingleOrDefault(user => user.GoogleId == googleId)
                .ProfilePicture;

            if (profilePicture != null)
                return profilePicture;
            else
                return string.Empty;
        }

        public async Task<List<Poll>> Index()
        {
            var domainPolls = await dbContext.Polls.Include(p => p.User).Include(p => p.Genre).ToListAsync();

           domainPolls = await this.GetVoteCounts(domainPolls);

           domainPolls = await this.GetProfilePictures(domainPolls);

            //one method to get polls
            //anothe rmethod to get vote count for each poll
            //another method to get profiel picture

 
            return domainPolls;
        }

        private async Task<List<Poll>> GetVoteCounts(List<Poll> polls)
        {
            var userpolls = await dbContext.UserPoll.ToListAsync();

            foreach (var poll in polls)
            {
                poll.FirstVoteCount = userpolls.Where(up => up.PollId == up.Id && up.Vote == true).Count();

                poll.SecondVoteCount = userpolls.Where(up => up.PollId == up.Id && up.Vote == false).Count();
            }
     


            //var polls = domainPolls.Select(p => new HomeViewModel
            //{
            //    Poll = mapper.Map<PollDto>(p),
            //    FirstVoteCount = dbContext
            //.UserPoll.Where(up => up.PollId == p.Id && up.Vote == true)
            //.Count(),
            //    SecondVoteCount = dbContext
            //.UserPoll.Where(up => up.PollId == p.Id && up.Vote == false)
            //.Count(),
            //    UserName = dbContext.User.SingleOrDefault(user => user.Id == p.UserId).Name,
            //    ProfilePicture = dbContext
            //.User.SingleOrDefault(user => user.Id == p.UserId)
            //.ProfilePicture
            //});


            return polls;
        }

        private async Task<List<Poll>> GetProfilePictures(List<Poll> polls)
        {

            foreach (var poll in polls)
            {
                var user = await dbContext.User.SingleOrDefaultAsync(user => user.Id == poll.UserId);
                poll.ProfilePicture = user.ProfilePicture;
            }

            return polls;
        }
    }
}
