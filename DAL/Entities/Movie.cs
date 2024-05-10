using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Movie: EntityBase
    {
        public int Id { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        public int year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string StoryLine { get; set; }
        [Required]
        public byte[] Poster { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }

    }
}
