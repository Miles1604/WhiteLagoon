using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Domain.Entities
//{
/*public class VillaNumber
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    //the database generated makes it so this isn't auto generated so we can get user to enter this as room number.
    //this makes it a primary key. normally don't have to add
          //if name is id or for example VillaID (model name followed by id)
          //as .net framework automatically spots this and assigns as primary key
    public int Villa_Number { get; set; }

    [ForeignKey("Villa")]
    public int VillaID { get; set; }
    public Villa? Villa { get; set; }
    public string? SpecialDetails { get; set; } //? makes it not mandatory and can be empty
}
}
*/


{
    public class VillaNumber
    {
        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Villa Number")]
        public int Villa_Number { get; set; }

        [ForeignKey("Villa")]
        public int VillaId { get; set; }
        [ValidateNever] //to get this to work. edit project file of domain folder. add the itemgroup framework reference.
        public Villa Villa { get; set; }
        public string? SpecialDetails { get; set; }
    }
}  