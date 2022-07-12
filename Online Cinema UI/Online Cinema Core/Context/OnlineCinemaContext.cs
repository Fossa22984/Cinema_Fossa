using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Online_Cinema_Domain.Models;
using Online_Cinema_Domain.Models.IdentityModels;
using System;

namespace Online_Cinema_Core.Context
{
    public class OnlineCinemaContext : IdentityDbContext<User, Role, Guid>
    {
        public OnlineCinemaContext(DbContextOptions<OnlineCinemaContext> options) : base(options) { }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<CinemaRoom> CinemaRooms { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasOne<Room>(x => x.Room)
                .WithOne(x => x.Owner)
                .HasForeignKey<Room>(x => x.OwnerId);

            builder.Entity<User>()
                .HasOne<Subscription>(x => x.Subscription)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.SubscriptionId);

            builder.Entity<User>()
                .HasMany<Comment>(x => x.Comments)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            builder.Entity<CinemaRoom>()
                .HasMany<Session>(x => x.Sessions)
                .WithOne(x => x.CinemaRoom)
                .HasForeignKey(x => x.CinemaRoomId);

            builder.Entity<Comment>()
                .HasOne<Movie>(x => x.Movie)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.MovieId);

            builder.Entity<Genre>()
                .HasMany<Movie>(x => x.Movies)
                .WithMany(x => x.Genres);

            builder.Entity<Movie>()
                .HasMany<Session>(x => x.Sessions)
                .WithOne(x => x.Movie)
                .HasForeignKey(x => x.MovieId);

            builder.Entity<Movie>()
                .HasMany<Room>(x => x.Rooms)
                .WithOne(x => x.Movie)
                .HasForeignKey(x => x.MovieId);
        }
    }
}