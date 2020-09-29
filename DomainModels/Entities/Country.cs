using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels.Entities
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        public string CountryName{ get; set; }
    }
}
