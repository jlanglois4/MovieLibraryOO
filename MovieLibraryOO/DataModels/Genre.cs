using System;
using System.Collections.Generic;

namespace MovieLibraryOO.DataModels
{
    public class Genre
    {
        // genre name and what movies each one is assigned to
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MovieGenre> MovieGenres {get;set;}
    }
}
