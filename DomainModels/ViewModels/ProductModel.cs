using DomainModels.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModels.Models
{
    [NotMapped]
    public class ProductModel : Product
    {
        public IFormFile file { get; set; }
    }
}
