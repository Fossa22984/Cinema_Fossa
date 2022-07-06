using System.Collections.Generic;

namespace Online_Cinema_Domain.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string GenreName { get; set; }
        public IList<Movie> Movies { get; set; }

        public Genre()
        {
            Movies = new List<Movie>();
        }

        public bool IsRemoved { get; set; }
    }
}