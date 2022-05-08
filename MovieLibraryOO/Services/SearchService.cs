using System;
using System.Collections.Generic;
using System.Linq;
using MovieLibraryOO.Context;
using MovieLibraryOO.DataModels;

namespace MovieLibraryOO.Services
{
    public class SearchService
    {
        private List<string> media = new List<string>();
        private string title;
        public List<Movie> MovieList { get; set; }

        public string SearchMedia(List<string> moviesDisplay)
        {
            var boolean = false;
            string? titleChoice;
            try
            {
                Console.WriteLine("Enter search term.");
                while (true)
                {
                    titleChoice = Console.ReadLine();
                    if (titleChoice != null)
                    {
                        title = titleChoice;
                        break;
                    }

                    Console.WriteLine("Enter a valid search term.");
                }

                int count = 0;
                foreach (var title in moviesDisplay.Where(title =>
                    title.Contains(titleChoice, StringComparison.CurrentCultureIgnoreCase)))
                {
                    media.Add(title);
                    count++;
                }

                Console.WriteLine(count + " matches");

                foreach (var result in media)
                {
                    Console.WriteLine(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Please enter a proper search term.");
                throw;
            }

            return title;
        }
        public int GetMatches(MovieContext _db, string type)
        {
            Console.WriteLine("Enter a title.");
            var movieName = Console.ReadLine();
            MovieList = new List<Movie>();
            if (movieName != "")
            {
                foreach (var mov in _db.Movies)
                {
                    if (mov.Title.Contains(movieName))
                    {
                        MovieList.Add(mov);
                    }
                }
            }

            int pickedChoice = 0;
            if (MovieList.Count != 0)
            {
                List<string> movieStringList = new List<string>();
                string movieString = "";
                Console.WriteLine($"Which movie would you like to {type}?\n");
                int i = 1;

                List<int> intCounter = new List<int>();
                foreach (var m in MovieList)
                {
                    movieString = $"{i}. {m.Title}";
                    movieStringList.Add(movieString);
                    Console.WriteLine(movieString);
                    intCounter.Add(i);
                    i++;
                }

                Console.WriteLine("\n" +
                                  "Enter anything else to go back.");


                try
                {
                    pickedChoice = Convert.ToInt32(Console.ReadLine());
                    if (!intCounter.Contains(pickedChoice))
                    {
                        throw new Exception();
                    }
                }
                catch (Exception e)
                {
                    Console.Write("Exiting: ");
                }
            }

            return pickedChoice;
        }
    }
}