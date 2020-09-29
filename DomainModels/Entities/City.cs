using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels.Entities
{
    public class City
    {
        [Key]
        public int CityId { get; set; }
        public string CityName { get; set; }

        [Display(Name = "State")]
        public int StateId { get; set; }

        [ForeignKey("StateId")]
        public virtual State State { get; set; }
    }
}
