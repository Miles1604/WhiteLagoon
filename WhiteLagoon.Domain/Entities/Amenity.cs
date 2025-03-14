using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Domain.Entities
{
	public class Amenity
	{
		[Key]
		public int Id { get; set; }
		
		public required string Name { get; set; }  //makes mandatory
		public string? Description { get; set; }

		[ForeignKey("Villa")]
		public int VillaId { get; set; }
		[ValidateNever] //to get this to work. edit project file of domain folder. add the itemgroup framework reference.
		public Villa Villa { get; set; }
		
	}
}
