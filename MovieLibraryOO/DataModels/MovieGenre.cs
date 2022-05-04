using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibraryOO.DataModels
{
    public class MovieGenre
    {
        
        // set of all movies with all the genres each movie has
    public int Id {get;set;}
    public virtual Movie Movie { get; set; }
    public virtual Genre Genre { get; set; }
    }
}



