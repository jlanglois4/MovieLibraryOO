using System;
using MovieLibraryOO.Context;
using MovieLibraryOO.DataModels;

namespace MovieLibraryOO.Services
{
    public class RateService
    {
        private MovieContext _db;
        public RateService(MovieContext db)
        {
            this._db = db;
        }

        public UserMovie RateMovie(User user, Movie movie)
        {
            bool isValid = false;
            Console.WriteLine($"What would you like to rate {movie.Title}? (1-5)");
            long rating = 0;
            do
            {
                try
                {
                    rating = Convert.ToInt32(Console.ReadLine());
                    if (rating is > 5 or < 1)
                    {
                        Console.WriteLine("Please enter a valid rating.");
                    }
                    else
                    {
                        isValid = true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            } while (!isValid);
            
            DateTime ratedAt = DateTime.Now;

            UserMovie userMovie = null;

            try
            {
                if (rating != 0)
                {
                    userMovie = new UserMovie {Rating = rating, RatedAt = ratedAt, User = user, Movie = movie};   
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error adding new rating.");
            }

            return userMovie;
        }
    }
}