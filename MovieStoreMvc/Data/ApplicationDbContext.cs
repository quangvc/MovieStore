using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieStoreMvc.Models;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace MovieStoreMvc.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Country> Country { get; set; }
        public DbSet<Format> Format { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Manufacturer> Manufacturer { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Rating> Rating { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Movie>(a =>
            {
                a.HasOne(b => b.manufacturer).WithMany().HasForeignKey(c => c.ManuId);
                a.HasOne(b => b.rating).WithMany().HasForeignKey(c => c.RatingId);
            });

            builder.Entity<Movie>()
                .HasMany(m => m.countries)
                .WithMany(c => c.movies);

            builder.Entity<Movie>()
                .HasMany(m => m.formats)
                .WithMany(c => c.movies);

            builder.Entity<Movie>()
                .HasMany(m => m.genres)
                .WithMany(c => c.movies);

            builder.Entity<Genre>(c =>
            {
                c.HasIndex(e => e.Name).IsUnique();
            });

            builder.Entity<Format>(c =>
            {
                c.HasIndex(e => e.Name).IsUnique();
            });

            builder.Entity<Country>(c =>
            {
                c.HasIndex(e => e.Name).IsUnique();
            });

            builder.Entity<Manufacturer>(c =>
            {
                c.HasIndex(e => e.Name).IsUnique();
            });

            builder.Entity<Rating>(c =>
            {
                c.HasIndex(e => e.Name).IsUnique();
            });

            builder.Entity<Room>(c =>
            {
                c.HasMany(i => i.Seats);
            });

            builder.Entity<Seat>()
                .HasIndex(s => new { s.Position, s.RoomId }).IsUnique();

            builder.Entity<Ticket>()
                .HasIndex(s => new { s.SeatId, s.ShowtimesId }).IsUnique();

            //builder.Entity<Seat>()
            //.HasKey(c => new { c.Position, c.RoomId });

            builder.Entity<Ticket>()
                .HasOne(e => e.showtimes)
                .WithMany()
                .HasForeignKey(c => c.ShowtimesId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<MovieStoreMvc.Models.RoomType> RoomType { get; set; }

        public DbSet<MovieStoreMvc.Models.SeatType> SeatType { get; set; }

        public DbSet<MovieStoreMvc.Models.Cinema> Cinema { get; set; }

        public DbSet<MovieStoreMvc.Models.Room> Room { get; set; }

        public DbSet<MovieStoreMvc.Models.Seat> Seat { get; set; }

        public DbSet<MovieStoreMvc.Models.Showtimes> Showtimes { get; set; }

        public DbSet<MovieStoreMvc.Models.Ticket> Ticket { get; set; }


    }
}