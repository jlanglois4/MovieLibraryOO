using System;
using System.Collections.Generic;
using MovieLibraryOO.Context;

namespace MovieLibraryOO
{
    public class MainMenu
    {
        public MainMenu()
        {
            MovieService movieDbService = new MovieService();
            var choice = true;
            do
            {
                if (movieDbService.exit)
                {
                    Console.WriteLine("Exiting program.");
                    return;
                }
                Console.WriteLine("\n" +
                                  "Welcome to the Movie Library.\n" +
                                  "1. List movies.\n" +
                                  "2. Add movie.\n" +
                                  "3. Search movies.\n" +
                                  "4. Modify movie.\n" +
                                  "5. Delete movie.\n" +
                                  "6. Add user.\n" +
                                  "7. Rate movie.\n" +
                                  "8. List top rated movie.\n" +
                                  "Enter anything else to exit the program.");
                var pickedChoice = Console.ReadLine();
                switch (pickedChoice)
                {
                    case "1":
                        movieDbService.DisplayMovie();
                        break;
                    case "2":
                        movieDbService.AddNewMovie();
                        break;
                    case "3":
                        movieDbService.Search();
                        break;
                    case "4":
                        movieDbService.UpdateMovie();
                        break;
                    case "5":
                        movieDbService.DeleteMovie();
                        break;
                    case "6":
                        movieDbService.AddUser();
                        break;
                    case "7":
                        movieDbService.RateMovie();
                        break;
                    case "8":
                        movieDbService.ListTopRatedMovies();
                        break;
                    default:
                        Console.WriteLine("Thank you for using the Media Library.");
                        choice = false;
                        break;
                }
            } while (choice);
        }
    }
}