﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RealTimePolls.Data;

#nullable disable

namespace RealTimePolls.Migrations
{
    [DbContext(typeof(RealTimePollsDbContext))]
    [Migration("20240528042155_Initial Migration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RealTimePolls.Models.Domain.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Genre");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Technology"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Science"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Health & Wellness"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Sports"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Music"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Literature"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Travel"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Food & Cooking"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Fashion"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Art & Design"
                        },
                        new
                        {
                            Id = 11,
                            Name = "Gaming"
                        },
                        new
                        {
                            Id = 12,
                            Name = "Education"
                        },
                        new
                        {
                            Id = 13,
                            Name = "Anime"
                        },
                        new
                        {
                            Id = 14,
                            Name = "Environment"
                        },
                        new
                        {
                            Id = 15,
                            Name = "Business & Finance"
                        },
                        new
                        {
                            Id = 16,
                            Name = "Movies & TV"
                        },
                        new
                        {
                            Id = 17,
                            Name = "Comedy"
                        },
                        new
                        {
                            Id = 18,
                            Name = "Lifestyle"
                        },
                        new
                        {
                            Id = 19,
                            Name = "History"
                        },
                        new
                        {
                            Id = 20,
                            Name = "DIY & Crafts"
                        });
                });

            modelBuilder.Entity("RealTimePolls.Models.Domain.Poll", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstOption")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("GenreId")
                        .HasColumnType("integer");

                    b.Property<string>("SecondOption")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.HasIndex("UserId");

                    b.ToTable("Polls");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FirstOption = "Chicken",
                            GenreId = 4,
                            SecondOption = "Egg",
                            Title = "Which came first?",
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            FirstOption = "First choice",
                            GenreId = 6,
                            SecondOption = "Second choice",
                            Title = "What is your option?",
                            UserId = 1
                        },
                        new
                        {
                            Id = 3,
                            FirstOption = "Summer",
                            GenreId = 18,
                            SecondOption = "Winter",
                            Title = "Which season do you prefer?",
                            UserId = 2
                        },
                        new
                        {
                            Id = 4,
                            FirstOption = "Apple",
                            GenreId = 1,
                            SecondOption = "Android",
                            Title = "Which smartphone brand do you prefer?",
                            UserId = 2
                        },
                        new
                        {
                            Id = 5,
                            FirstOption = "Pizza",
                            GenreId = 8,
                            SecondOption = "Burger",
                            Title = "Favorite fast food?",
                            UserId = 3
                        });
                });

            modelBuilder.Entity("RealTimePolls.Models.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("GoogleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProfilePicture")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "user1@gmail.com",
                            GoogleId = "1111111",
                            Name = "User One",
                            ProfilePicture = "https://picsum.photos/500"
                        },
                        new
                        {
                            Id = 2,
                            Email = "user2@gmail.com",
                            GoogleId = "2222222",
                            Name = "User Two",
                            ProfilePicture = "https://picsum.photos/500"
                        },
                        new
                        {
                            Id = 3,
                            Email = "user3@gmail.com",
                            GoogleId = "3333333",
                            Name = "User Three",
                            ProfilePicture = "https://picsum.photos/500"
                        });
                });

            modelBuilder.Entity("RealTimePolls.Models.Domain.UserPoll", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("PollId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<bool?>("Vote")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("UserPoll");
                });

            modelBuilder.Entity("RealTimePolls.Models.Domain.Poll", b =>
                {
                    b.HasOne("RealTimePolls.Models.Domain.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealTimePolls.Models.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
