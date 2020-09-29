using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels.Entities
{
    public class User : IdentityUser<int>
    {
        [Required(ErrorMessage ="Please Enter Name")]
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        [NotMapped]
        public string Password { get; set; }
        [NotMapped]
        public string ConfirmPassword { get; set; }
    }
}
