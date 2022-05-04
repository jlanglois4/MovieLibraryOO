using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MovieLibraryOO.Context;
using MovieLibraryOO.DataModels;

namespace MovieLibraryOO
{
    public class GenreService
    {
        private MovieContext _db;
    
        private Genre genre;

        public GenreService(MovieContext db)
        {
            this._db = db;
        }
        public Genre GetGenre(string input)
        {
            switch (input)
                {
                    case "1":
                        var boolean = true;
                        do
                        {
                            Console.Write("Enter genre: ");
                            string genreInput = Console.ReadLine();

                            if (genreInput != "")
                            {
                                foreach (var dbGenre in _db.Genres)
                                {
                                    if (!dbGenre.Name.Contains(genreInput)) continue;
                                    genre = dbGenre;
                                    boolean = false;
                                    break;
                                }
                                if (genre is null)
                                {
                                    genre = new Genre();
                                    genre.Name = genreInput;
                                }
                            }
                            else
                            {
                                Console.Write("Please enter a genre.");
                            }
                        } while (boolean);
                        break;
                    
                    case "2":
                        Console.WriteLine("Exit");
                        if (genre is null)
                        {
                            string genreInput = "N/A";
                            
                            foreach (var dbGenre in _db.Genres)
                            {
                                if (!dbGenre.Name.Contains(genreInput)) continue;
                                genre = dbGenre;
                                break;
                            }

                            if (genre is null)
                            {
                                genre = new Genre();
                                genre.Name = "N/A";
                            }
                        }
                        break;
                }
            return genre;
        }

        public Genre GetNewGenre()
        {
            return GenreIsUnique(genre.Name) ? genre : null;
        }
        private bool GenreIsUnique(string genreInput)
        {
            foreach (var dbGenre in _db.Genres)
            {
                if (dbGenre.Name.Contains(genreInput))
                {
                    return false;
                }
            }
            return true;
        }
    }
}