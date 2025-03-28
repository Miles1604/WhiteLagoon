﻿using Microsoft.AspNetCore.Http; //needed to use IFormFile
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        [ValidateNever] //add this validate never else it will try and validate the below in model state validation.
        public IEnumerable<Amenity> VillaAmenity { get; set; }
        //to populate villa amenity go to home controller and put in the get all brackets

        [NotMapped] //doesn't create in db
        public bool IsAvailable { get; set; } = true;

    }
}
