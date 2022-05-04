using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MovieLibraryOO.Context;
using MovieLibraryOO.DataModels;
using MovieLibraryOO.Services;

namespace MovieLibraryOO
{
    public class MovieService
    {
        private MovieContext _db = new MovieContext();
        private const string NEWLINE = "\n";

        private List<Movie> _dbMovies;

        private List<Genre> _dbGenres;

        private List<MovieGenre> _dbMovieGenres;

        private List<string> _displayMovies = new List<string>();

        private DateTime releaseDate;

        private List<Genre> _genreList;

        private Movie _movie;

        private MovieGenre _movieGenre;


        public MovieService()
        {
            // Loads all the movies from the database and puts them into a console friendly list
            try
            {
                SetDatabaseMovies();
                SetDatabaseGenres();
                InitializeMovieDisplay();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void SetDatabaseGenres()
        {
            this._dbGenres = _db.Genres.OrderBy(b => b.Id).ToList();
        }

        private void SetDatabaseMovies()
        {
            this._dbMovies = _db.Movies.OrderBy(b => b.Id).ToList();
        }

        public void DisplayMovie()
        {
            // Display all Movies from the database
            try
            {
                MediaReadService mediaReadService = new MediaReadService();
                mediaReadService.ListMedia(_displayMovies);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void AddNewMovie()
        {
            try
            {
                // Create and save a new Movie

                var title = MovieNameValidation();
                Console.WriteLine(releaseDate);

                var movieId = _dbMovies.Count() + 1;
                //var movieId = 1;
                _movie = new Movie {Title = title, ReleaseDate = releaseDate};
                SetGenres();
                SetMovieGenre();
                _db.AddMovieGenre(_movieGenre);


                var finalMovie = new Movie
                    {Id = movieId, Title = title, ReleaseDate = releaseDate, MovieGenres = _dbMovieGenres};

                _dbMovies.Add(finalMovie);
                _displayMovies.Add(finalMovie.Display());

                Console.WriteLine("Movie added - " +
                                  $"{movieId}\n" +
                                  $"{title}\n" +
                                  $"{releaseDate}\n" +
                                  $"{string.Join(" | ", _genreList)}\n" +
                                  $"{string.Join(" | ", _dbMovieGenres)}\n");

                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void InitializeMovieDisplay()
        {
            if (_displayMovies.Count == 0)
            {
                Console.WriteLine("Loading all movies...");
                _displayMovies = _dbMovies.Select(m => m.Display()).ToList();
            }
        }

        public void Search()
        {
            try
            {
                bool b = false;
                while (!b)
                {
                    SearchService searchService = new SearchService();
                    searchService.SearchMedia(_displayMovies);

                    Console.WriteLine("1. Search again.\n" +
                                      "Enter anything else to exit.");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            break;
                        default:
                            b = true;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private DateTime MovieReleaseDate()
        {
            DateTime releaseDate;

            Console.WriteLine("Date the movie released: month/day/year");

            if (DateTime.TryParse(Console.ReadLine(), out releaseDate))
            {
                this.releaseDate = releaseDate;
            }
            else
            {
                Console.WriteLine("Please use the format: month/day/year");
            }

            return releaseDate;
        }

        private string MovieNameValidation()
        {
            string title = null;
            try
            {
                bool boolean;

                if (_dbMovies.Count != 0)
                {
                    do
                    {
                        boolean = false;
                        Console.WriteLine("Enter movie title: ");
                        title = Console.ReadLine();

                        foreach (var movie in _dbMovies)
                        {
                            if (movie.Title.Substring(0, movie.Title.Length - 7).Contains(title))
                            {
                                Console.WriteLine("Please enter a unique movie title." + NEWLINE);
                                boolean = true;
                                break;
                            }
                        }
                    } while (boolean);
                }
                else
                {
                    Console.WriteLine("There are no entries.");
                    Thread.Sleep(500);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var year = MovieReleaseDate().ToString("yyyy");
            return $"{title} ({year})";
        }

        public void UpdateMovie()
        {
            /*SearchService searchService = new SearchService();
            var movieName = searchService.SearchMedia(_displayMovies);*/

            Console.WriteLine("Enter a title.");
            var movieName = Console.ReadLine();
            List<Movie> movieList = new List<Movie>();
            if (movieName != "")
            {
                foreach (var mov in _db.Movies)
                {
                    if (mov.Title.Contains(movieName))
                    {
                        movieList.Add(mov);
                    }
                }
            }


            if (movieList.Count != 0)
            {
                List<string> movieStringList = new List<string>();
                string movieString = "";
                Console.WriteLine($"Which movie would you like to update?{NEWLINE}");
                int i = 1;

                foreach (var m in movieList)
                {
                    movieString = $"{i}. {m.Title}";
                    movieStringList.Add(movieString);
                    Console.WriteLine(movieString);
                    i++;
                }

                Console.WriteLine($"{NEWLINE}" +
                                  "Enter anything else to go back.");

                int pickedChoice;
                try
                {
                    pickedChoice = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception e)
                {
                    pickedChoice = 0;
                }

                if (pickedChoice != 0)
                {
                    Movie movie = movieList[pickedChoice - 1];
                    Movie old = movie;

                    Console.WriteLine(NEWLINE + movie.Display());

                    Console.WriteLine($"What would you like to modify?{NEWLINE}" +
                                      $"1. Title{NEWLINE}" +
                                      $"2. Genres{NEWLINE}" +
                                      "Enter anything else to exit.");

                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            _displayMovies.Remove(old.Display());
                            string newTitle = MovieNameValidation();
                            movie.Title = newTitle;
                            movie.ReleaseDate = releaseDate;
                            _db.UpdateMovie(movie);
                            _displayMovies.Add(movie.Display());
                            Console.WriteLine($"Updated movie title.{NEWLINE}");
                            break;
                        case "2":
                            _displayMovies.Remove(old.Display());
                            _movie = movie;
                            SetGenres();
                            SetMovieGenre();
                            _db.DeleteMovieGenre(_movie);

                            foreach (var mg in _dbMovieGenres)
                            {
                                _db.UpdateMovieGenre(mg);
                            }

                            _movie.MovieGenres = _dbMovieGenres;
                            _displayMovies.Add(_movie.Display());

                            Console.WriteLine($"Updated movie genres.{NEWLINE}");
                            break;
                    } 
                }
            }
            else
            {
                Console.WriteLine("No match.");
            }
        }

        private void SetMovieGenre()
        {
            _dbMovieGenres = new List<MovieGenre>();

            foreach (var g in _genreList)
            {
                _movieGenre = new MovieGenre {Genre = g, Movie = _movie};
                _dbMovieGenres.Add(_movieGenre);
            }
        }

        private void SetGenres()
        {
            GenreService genreService = new GenreService(_db);
            _genreList = new List<Genre>();

            while (true)
            {
                Console.WriteLine(string.Format("1: Enter genre\n" +
                                                "2: Exit"));
                string input = Console.ReadLine();

                Genre genre = genreService.GetGenre(input);


                if (genre.Name == "N/A" && _genreList.Count == 0)
                {
                    _genreList.Add(genre);
                }

                if (genreService.GetNewGenre() is not null)
                {
                    _dbGenres.Add(genreService.GetNewGenre());
                }

                if (input == "2")
                {
                    break;
                }

                _genreList.Add(genre);
            }
        }

        public void DeleteMovie()
        {
            Console.WriteLine("Enter movie to delete.");
            string input = Console.ReadLine();
            SetDatabaseMovies();
            int detected = 0;
            foreach (var movie in _dbMovies)
            {
                if (movie.Title.Substring(0, movie.Title.Length - 7) == input)
                {
                    detected = 1;
                    _displayMovies.Remove(movie.Display());
                    _db.DeleteMovieGenre(movie);
                    _db.DeleteMovie(movie);
                    Console.WriteLine("Movie deleted.");
                }
            }

            if (detected == 0)
            {
                Console.WriteLine("Invalid movie.");
            }
        }
    }
}