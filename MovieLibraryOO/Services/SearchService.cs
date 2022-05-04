using System;
using System.Collections.Generic;
using System.Linq;
using MovieLibraryOO.Context;

namespace MovieLibraryOO.Services
{
    public class SearchService
    {
        private List<string> media = new List<string>();
        private string title;

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
    }
}