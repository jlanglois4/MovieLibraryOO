using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MovieLibraryOO.Context;
using MovieLibraryOO.DataModels;
using MovieLibraryOO.Services;
using NLog;

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
        private DateTime releaseDate = new DateTime();
        private List<Genre> _genreList;
        private Movie _movie;
        private MovieGenre _movieGenre;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public bool exit { get; set; }


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
                Console.WriteLine("Error initializing movies.");
                logger.Error(e);
                exit = true;
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
                new ContinueService();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error displaying movies.");
                logger.Error(e);
            }
        }

        public void AddNewMovie()
        {
            try
            {
                // Create and save a new Movie

                var title = MovieNameValidation();

                
                _movie = new Movie {Title = title, ReleaseDate = releaseDate};
                SetGenres();
                SetMovieGenre();
                var movieId = _db.Movies.Max(mov => mov.Id);
                _db.AddMovieGenre(_movieGenre);

                
                var finalMovie = new Movie
                    {Id = movieId, Title = title, ReleaseDate = releaseDate, MovieGenres = _dbMovieGenres};

                _dbMovies.Add(finalMovie);
                _displayMovies.Add(finalMovie.Display());

                Console.WriteLine("\nMovie added:\n" +
                                  finalMovie.Display());

                new ContinueService();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error adding new movie.");
                logger.Error(e);
            }
        }

        private void InitializeMovieDisplay()
        {
            try
            {
                if (_displayMovies.Count == 0)
                {
                    Console.WriteLine("Loading all movies...");
                    _displayMovies = _dbMovies.Select(m => m.Display()).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error initializing movies.");
                logger.Error(e);
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
                Console.WriteLine("Error searching for movie.");
                logger.Error(e);
            }
        }

        private DateTime MovieReleaseDate()
        {
            try
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
            }
            catch (Exception e)
            {
                Console.WriteLine("Error getting release date.");
                logger.Error(e);
            }

            return releaseDate;
        }

        private string MovieNameValidation()
        {
            string year = null;
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

                year = MovieReleaseDate().ToString("yyyy");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error setting movie title.");
                logger.Error(e);
            }


            return $"{title} ({year})";
        }

        public void UpdateMovie()
        {
            try
            {
                SearchService searchService = new SearchService();
                int pickedChoice = searchService.GetMatches(_db, "update");

                List<Movie> movieList = searchService.MovieList;
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
                else
                {
                    Console.WriteLine("No match.");
                }

                new ContinueService();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error updating movie.");
                logger.Error(e);
            }
        }


        private void SetMovieGenre()
        {
            try
            {
                _dbMovieGenres = new List<MovieGenre>();
                foreach (var g in _genreList)
                {
                    _movieGenre = new MovieGenre {Genre = g, Movie = _movie};
                    _dbMovieGenres.Add(_movieGenre);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error setting MovieGenre.");
                logger.Error(e);
            }
        }

        private void SetGenres()
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine("Error setting movie genres.");
                logger.Error(e);
            }
        }

        public void DeleteMovie()
        {
            try
            {
                do
                {
                    string input = "";
                    do
                    {
                        Console.WriteLine("Enter movie to delete.");
                        input = Console.ReadLine();

                        if (input == "")
                        {
                            Console.WriteLine("Movie cannot be blank.");
                        }
                    } while (input == "");
                    
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
                            break;
                        }
                    }

                    if (detected == 0)
                    {
                        Console.WriteLine("Invalid movie.");
                    }
                } while (exit);
                

                new ContinueService();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error deleting movie.");
                logger.Error(e);
            }
        }

// Add new user including occupation
// and display the details of the added user after adding
        public void AddUser()
        {
            try
            {
                UserService userService = new UserService(_db);
                userService.AddUser();
                new ContinueService();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error adding user.");
                logger.Error(e);
            }
        }

// Ask for user to enter a rating on an exisiting movie and
// display the details of the user, rated movie and rating


        public void RateMovie()
        {
            try
            {
                bool validChoice = false;
                long userId = 0;
                User user = null;

                do
                {
                    do
                    {
                        Console.WriteLine("Please enter your User ID.");
                        try
                        {
                            userId = Convert.ToInt32(Console.ReadLine());
                            validChoice = true;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Please enter a valid User ID.");
                            Console.WriteLine(e);
                        }
                    } while (!validChoice);

                    foreach (var u in _db.Users)
                    {
                        if (u.Id == userId)
                        {
                            user = u;
                        }
                    }

                    do
                    {
                        validChoice = false;

                        if (user == null)
                        {
                            Console.WriteLine("User ID not found. Would you like to create a new user ID? (Y/N)");
                            char newUserPrompt;

                            newUserPrompt = Convert.ToChar(Console.ReadLine().ToUpper());
                            if (newUserPrompt == 'Y')
                            {
                                AddUser();
                                userId = 0;
                                break;
                            }

                            if (newUserPrompt != 'N')
                            {
                                Console.WriteLine("Enter a valid response.");
                            }
                            else if (newUserPrompt == 'N')
                            {
                                Console.WriteLine("Exiting.");
                                return;
                            }
                        }
                        else
                        {
                            validChoice = true;
                        }
                    } while (!validChoice);
                } while (userId == 0);

                SearchService searchService = new SearchService();
                var movieChoice = searchService.GetMatches(_db, "review");
                List<Movie> movieList = null;
                try
                {
                    while (movieList == null)
                    {
                        movieList = searchService.MovieList;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }

                Movie movie = movieList[movieChoice - 1];
                RateService rateService = new RateService(_db);
                UserMovie userMovie = rateService.RateMovie(user, movie);
                _db.AddUserMovie(userMovie);
                Console.WriteLine($"{NEWLINE}Review added:");
                Console.WriteLine(userMovie.Display());
                new ContinueService();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error rating movie.");
                logger.Error(e);
            }
        }

//sort alphabetically and by rating
//show top rated movie by age bracket or occupation
//display only the first top movie
        public void ListTopRatedMovies()
        {
            try
            {
                Console.WriteLine($"What would you like to sort the top rating by:{NEWLINE}" +
                                  $"1. Age bracket{NEWLINE}" +
                                  $"2. Occupation{NEWLINE}");

                var input = Console.ReadLine();

                RatingSortService ratingSortService = new RatingSortService(_db);
                switch (input)
                {
                    case "1":
                        ratingSortService.PickAgeRange();
                        break;
                    case "2":
                        ratingSortService.GetTopRatedMovie(true, 0, 0);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error listing top rated movie.");
                logger.Error(e);
            }
        }
    }
}