using System;
using System.Collections.Generic;

namespace MovieLibraryOO.DataModels
{
    public class User
    {
        // User information, age, gender, zipcode, occupation (name, occupation), and list of movies rated by the user
        public long Id { get; set; }
        public long Age { get; set; }
        public string Gender { get; set; }
        public string ZipCode { get; set; }

        public virtual Occupation Occupation { get; set; }
        public virtual ICollection<UserMovie> UserMovies {get;set;}
    }
}
