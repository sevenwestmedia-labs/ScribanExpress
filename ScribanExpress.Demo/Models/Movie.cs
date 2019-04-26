using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScribanExpress.Demo.Models
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public List<string> Genres { get; set; }
        public decimal Price { get; set; }
        public bool IsFeatured { get; set; }

        public Classifaction Classification { get; set; }
        public string TallImageUrl { get; set; }
    }

    public enum Classifaction
    {
        G,
        M,
        AO
    }
}
