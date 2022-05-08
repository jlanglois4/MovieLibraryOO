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


        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
        public virtual ICollection<UserMovie> UserMovies { get; set; }


        public string Display()
        {
            List<string> genreList = MovieGenres.Select(genre => genre.Genre.Name).ToList();

            return $"ID: {Id}\n" +
                   $"Title: {Title}\n" +
                   $"Genres: {string.Join(" | ", genreList)}";
        }
    }
}