using Microsoft.AspNetCore.Http; //needed to use IFormFile
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Domain.Entities
{
    public class Villa
    {
        public int Id { get; set; }
        [MaxLength(50)]
        //Another type of data annotation. Stops adding characters after 50 is reached.
        public required string Name { get; set; }
        public string? Description { get; set; }


        [Display(Name = "Price per night")]
        [Range(10, 10000)]
        public double Price { get; set; }
        [Range(10, 10000)]
        //Another type of data annotation. Minimum to maxium. Works alongside server validation
        public int Sqft {  get; set; }
        [Range(1, 10)]
        public int Occupancy { get; set; }


        //The below is a data annotation. ImageUrl will now show as Image Url
        //When typing the below code, right click Display and add System
        [NotMapped] //this is telling the app not to add the IFormFile below to the database
        public IFormFile? Image { get; set; } //Need the using statement of aspnetcore http at the very top to work with IFormFile
        [Display(Name= "Image Url")]
        public string? ImageUrl { get; set; }
        public DateTime? Created_Date { get; set; }
        public DateTime? Updated_Date { get;set; }

    }
}
