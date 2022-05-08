using System;
using System.Collections.Generic;

namespace MovieLibraryOO.DataModels
{
    // Ratings for a movie by a user
    public class UserMovie
    {
        public long Id { get; set; }
        public long Rating { get; set; }
        public DateTime RatedAt { get; set; }

        public virtual User User { get; set; }
        public virtual Movie Movie { get; set; }

        public string Display()
        {
            return $"User ID: {User.Id}\n" +
                   $"Gender: {User.Gender}\n" +
                   $"Occupation: {User.Occupation.Name}\n" +
                   $"Movie: {Movie.Title}\n" +
                   $"Rating ID: {Id}\n" +
                   $"Rating: {Rating}\n" +
                   $"Rated At: {RatedAt}";
        }
    }
}