﻿using Microsoft.EntityFrameworkCore;
using RealTimePolls.Models.Domain;

namespace RealTimePolls.Data
{
    public class RealTimePollsContext : DbContext
    {
        public RealTimePollsContext(DbContextOptions<RealTimePollsContext> options)
            : base(options) { }

        public DbSet<Poll> Polls { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserPoll> UserPoll { get; set; }
        public DbSet<Genre> Genre { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var genres = new List<Genre>
            {
                new Genre { Id = 1, Name = "Technology" },
                new Genre { Id = 2, Name = "Science" },
                new Genre { Id = 3, Name = "Health & Wellness" },
                new Genre { Id = 4, Name = "Sports" },
                new Genre { Id = 5, Name = "Music" },
                new Genre { Id = 6, Name = "Literature" },
                new Genre { Id = 7, Name = "Travel" },
                new Genre { Id = 8, Name = "Food & Cooking" },
                new Genre { Id = 9, Name = "Fashion" },
                new Genre { Id = 10, Name = "Art & Design" },
                new Genre { Id = 11, Name = "Gaming" },
                new Genre { Id = 12, Name = "Education" },
                new Genre { Id = 13, Name = "Anime" },
                new Genre { Id = 14, Name = "Environment" },
                new Genre { Id = 15, Name = "Business & Finance" },
                new Genre { Id = 16, Name = "Movies & TV" },
                new Genre { Id = 17, Name = "Comedy" },
                new Genre { Id = 18, Name = "Lifestyle" },
                new Genre { Id = 19, Name = "History" },
                new Genre { Id = 20, Name = "DIY & Crafts" }
            };

            modelBuilder.Entity<Genre>().HasData(genres);
        }
    }
}