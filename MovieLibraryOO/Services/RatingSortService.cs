using System;
using System.Collections.Generic;
using System.Linq;
using MovieLibraryOO.Context;
using MovieLibraryOO.DataModels;

namespace MovieLibraryOO.Services
{
    public class RatingSortService
    {
        private MovieContext _db;
        private IDictionary<long, double> _movieAvgRating;

        public RatingSortService(MovieContext db)
        {
            this._db = db;
        }

        public void PickAgeRange()
        {
            Console.WriteLine("Which age bracket?\n" +
                              "1. 18-25\n" +
                              "2. 26-40\n" +
                              "3. 41-60\n" +
                              "4. 61+\n" +
                              "Enter anything else to exit.");

            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    GetTopRatedMovie(false, 18, 25);
                    break;
                case "2":
                    GetTopRatedMovie(false, 26, 40);
                    break;
                case "3":
                    GetTopRatedMovie(false, 41, 60);
                    break;
                case "4":
                    GetTopRatedMovie(false, 61, int.MaxValue);
                    break;
                default:
                    Console.WriteLine("Exiting.");
                    return;
            }
        }

        public void GetTopRatedMovie(bool option, int minAge, int maxAge)
        {
            string occupationName = null;
            IQueryable<string> query;
            if (option)
            {
                OccupationService occupationService = new OccupationService(_db);
                occupationName = occupationService.GetOccupationName();
                query =
                    from userMovie in _db.UserMovies
                    join movie in _db.Movies on userMovie.Movie.Id equals movie.Id
                    join user in _db.Users on userMovie.User.Id equals user.Id
                    where user.Occupation.Name == occupationName
                    orderby movie.Title
                    select movie.Id + "|" + userMovie.Rating;
            }
            else
            {
                query = // implement min and max age
                    from userMovie in _db.UserMovies
                    join movie in _db.Movies on userMovie.Movie.Id equals movie.Id
                    join user in _db.Users on userMovie.User.Id equals user.Id
                    where user.Age > minAge && user.Age < maxAge
                    orderby movie.Title
                    select movie.Id + "|" + userMovie.Rating;
            }


            var listOfMovieIds = new List<string>();
            IDictionary<string, double> movieIdRating = new Dictionary<string, double>();
            IDictionary<string, double> movieIDCount = new Dictionary<string, double>();
            var listOfLists = new List<List<string>>();

            foreach (var item in query)
            {
                List<string> thing = item.Split("|").ToList();
                listOfLists.Add(thing);
                if (!listOfMovieIds.Contains(thing[0]))
                {
                    listOfMovieIds.Add(thing[0]);
                }
            }

            foreach (var integer in listOfMovieIds)
            {
                movieIdRating.Add(integer, 0);
                movieIDCount.Add(integer, 0);
            }


            foreach (var list in listOfLists)
            {
                movieIdRating[list[0]] += Convert.ToInt32(list[1]);
                movieIDCount[list[0]] += 1;
            }


            List<double> averages = new List<double>();
            foreach (var (key, value) in movieIdRating)
            {
                averages.Add(movieIdRating[key] = value * 1.0 / movieIDCount[key]);
            }


            var final =
                from pair in movieIdRating
                where pair.Value == movieIdRating.Max(item => item.Value)
                select pair.Key + " " + pair.Value;


            var topMovie = final.First().Split(" ");

            var movieId = Convert.ToInt32(topMovie[0]);
            var movieRating = topMovie[1];
            Movie finalMovie = _db.Movies.First(mov => mov.Id.Equals(movieId));


            AverageRatedMovie averageRatedMovie = new AverageRatedMovie
                {AvgRating = Convert.ToDouble(movieRating), Movie = finalMovie};

            Console.WriteLine();
            switch (option)
            {
                case true:
                    Console.WriteLine($"Top rated movie amongst {occupationName}s: ");
                    break;
                case false:
                    Console.WriteLine(maxAge == Int32.MaxValue
                        ? $"Top rated movie from age {minAge} and up:"
                        : $"Top rated movie from ages {minAge} to {maxAge}:");
                    break;
            }


            Console.WriteLine(averageRatedMovie.Display());
            new ContinueService();
        }


        private class AverageRatedMovie
        {
            //  public int MovieId { get; set; }
            public double AvgRating { get; set; }
            public Movie Movie { get; set; }

            public string Display()
            {
                List<string> genreList = Movie.MovieGenres.Select(genre => genre.Genre.Name).ToList();

                return $"ID: {Movie.Id}\n" +
                       $"Title: {Movie.Title}\n" +
                       $"Genres: {string.Join(" | ", genreList)}\n" +
                       $"Average Rating: {AvgRating}";
            }
        }
    }
}