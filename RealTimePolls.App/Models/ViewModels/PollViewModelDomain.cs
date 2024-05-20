﻿using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;

namespace RealTimePolls.Models.ViewModels
{
    public class PollViewModelDomain
    {
        public int FirstVoteCount { get; set; }
        public int SecondVoteCount { get; set; }
        public bool? Vote { get; set; }

        //Navigation properties
        public Poll Poll { get; set; }
    }
}
