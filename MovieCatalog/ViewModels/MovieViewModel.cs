using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieCatalog.ViewModels
{
    public class MovieViewModel
    {

        public int Id { get; set; }

        /// <summary>
        /// Movie name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Movie description
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Movie released date
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime DateReleased { get; set; }

        /// <summary>
        /// Movie director
        /// </summary>
        [Required]
        public string Director { get; set; }

        /// <summary>
        /// User, who posted movie
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Movie poster
        /// </summary>
        public byte[] Poster { get; set; }
    }
}
