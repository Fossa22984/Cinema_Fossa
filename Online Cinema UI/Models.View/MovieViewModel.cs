using Microsoft.AspNetCore.Http;
using Online_Cinema_Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Online_Cinema_Models.View
{
    public class MovieViewModel
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        [/*Required,*/ Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        [Required, Display(Name = "Video")]
        public IFormFile VideoFile { get; set; }

        [Required, Display(Name = "Movie Title")]
        public string MovieTitle { get; set; }

        [Required, Display(Name = "Movie Path")]
        public string MoviePath { get; set; }

        [Required, Display(Name = "Date Of Release")]
        public DateTime? DateOfRelease { get; set; }//Дата выпуска 

        //public double Duration { get; set; }//Продолжительность 
        public string Author { get; set; }
        public string Actors { get; set; }
        public string Country { get; set; }

        [Display(Name = "Age Limit")]
        public int? AgeLimit { get; set; }//Возрастное ограничение
        public string Description { get; set; }//Описание

        [Display(Name = "Movie Budget")]
        public decimal? MovieBudget { get; set; }//Бюджет фильма 

        [Display(Name = "Cartoon?")]
        public bool IsCartoon { get; set; }

        [Display(Name = "Is Removed?")]
        public bool IsRemoved { get; set; }
        public IList<GenreViewModel> Genres { get; set; }//Жанр 
        public MovieViewModel()
        {
            Genres = new List<GenreViewModel>();
        }
    }

}
