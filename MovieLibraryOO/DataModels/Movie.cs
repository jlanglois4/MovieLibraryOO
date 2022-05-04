using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieLibraryOO.DataModels
{
    public class Movie
    {
        // contains title and release date, list of all genres for a movie, and list of all ratings of the movie
        
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }

        
        public virtual ICollection<MovieGenre> MovieGenres {get;set;}
        public virtual ICollection<UserMovie> UserMovies {get;set;}
        
        
        public string Display()
        {
            List<string> genreList = MovieGenres.Select(genre => genre.Genre.Name).ToList();

            /*var chars = Genres.ToList();
            Console.WriteLine($"Genres: {string.Join(" | ", chars)}\n");*/

            /*ICollection<MovieGenre> movieGenre = MovieGenres;
            
            List<string> genreList = new List<string>();
            foreach (var mg in movieGenre)
            {
                genreList.Add(mg.Genre.Name);
            }*/
            
            return $"ID: {Id}\n" +
                   $"Title: {Title}\n" +
                   $"Genres: {string.Join(" | ", genreList)}\n";
        }
    }
}
