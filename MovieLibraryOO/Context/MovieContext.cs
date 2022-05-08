using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieLibraryOO.DataModels;

namespace MovieLibraryOO.Context
{
    public class MovieContext : DbContext
    {
        public DbSet<Genre> Genres {get;set;}
        public DbSet<Movie> Movies {get;set;}
        public DbSet<MovieGenre> MovieGenres {get;set;}
        public DbSet<Occupation> Occupations {get;set;}
        public DbSet<User> Users {get;set;}
        public DbSet<UserMovie> UserMovies {get;set;}
        public void AddMovieGenre(MovieGenre movieGenre)
        {
            this.MovieGenres.Add(movieGenre);
            this.SaveChanges();
        }

        public void AddUser(User user)
        {
            this.Users.Add(user);
            this.SaveChanges();
        }
        
        public void AddUserMovie(UserMovie userMovie)
        {
            this.UserMovies.Add(userMovie);
            this.SaveChanges();
        }

        public void UpdateMovie(Movie movie)
        {
            this.Movies.Update(movie);
            this.SaveChanges();
        }
        
        public void UpdateMovieGenre(MovieGenre movieGenre)
        {
            this.MovieGenres.Update(movieGenre);
            this.SaveChanges();
        }
        
        public void DeleteMovie(Movie movie)
        {
            this.Movies.Remove(movie);
            this.SaveChanges();
        }
        
        public void DeleteMovieGenre(Movie movie)
        {
            foreach (var mg in movie.MovieGenres)
            {
                this.MovieGenres.Remove(mg);
            }
            this.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("MovieContext"));
        }
    }
}