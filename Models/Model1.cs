using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WbeMovieUser.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<actor> actors { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ChatMessage> ChatMessages { get; set; }
        public virtual DbSet<comment> comments { get; set; }
        public virtual DbSet<director> directors { get; set; }
        public virtual DbSet<episode> episodes { get; set; }
        public virtual DbSet<movy> movies { get; set; }
        public virtual DbSet<MovieView> MovieViews { get; set; }
        public virtual DbSet<Nation> Nations { get; set; }
        public virtual DbSet<rating> ratings { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<video> videos { get; set; }
        public virtual DbSet<watch_history> watch_history { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<actor>()
                .Property(e => e.bio)
                .IsUnicode(false);

            modelBuilder.Entity<actor>()
                .HasMany(e => e.movies)
                .WithMany(e => e.actors)
                .Map(m => m.ToTable("movie_actors").MapLeftKey("actor_id").MapRightKey("movie_id"));

            modelBuilder.Entity<Category>()
                .HasMany(e => e.movies)
                .WithMany(e => e.Categories)
                .Map(m => m.ToTable("MovieCategories").MapLeftKey("category_id").MapRightKey("movie_id"));

            modelBuilder.Entity<comment>()
                .Property(e => e.content)
                .IsUnicode(false);

            modelBuilder.Entity<director>()
                .Property(e => e.bio)
                .IsUnicode(false);

            modelBuilder.Entity<director>()
                .HasMany(e => e.movies)
                .WithMany(e => e.directors)
                .Map(m => m.ToTable("movie_directors").MapLeftKey("director_id").MapRightKey("movie_id"));

            modelBuilder.Entity<episode>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<movy>()
                .Property(e => e.thumbnail_url)
                .IsUnicode(false);

            modelBuilder.Entity<movy>()
                .HasMany(e => e.episodes)
                .WithRequired(e => e.movy)
                .HasForeignKey(e => e.series_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<movy>()
                .HasMany(e => e.MovieViews)
                .WithRequired(e => e.movy)
                .HasForeignKey(e => e.MovieId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<movy>()
                .HasMany(e => e.ratings)
                .WithRequired(e => e.movy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<movy>()
                .HasMany(e => e.Nations)
                .WithMany(e => e.movies)
                .Map(m => m.ToTable("MovieNations").MapLeftKey("movie_id").MapRightKey("nation_id"));

            modelBuilder.Entity<MovieView>()
                .Property(e => e.IpAddress)
                .IsUnicode(false);

            modelBuilder.Entity<rating>()
                .Property(e => e.rating_value)
                .HasPrecision(3, 1);

            modelBuilder.Entity<user>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.ratings)
                .WithRequired(e => e.user)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.watch_history)
                .WithRequired(e => e.user)
                .WillCascadeOnDelete(false);
        }
    }
}
