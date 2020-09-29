using DomainModels.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModels.Models
{
    public class CompanyModel
    {
        public CompanyModel()
        {
            CreatedDate = DateTime.Now;
        }
        [Key]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Please Enter Company")]
        [Column(TypeName = "varchar(50)")]
        public string CompanyName { get; set; }

        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Please Enter DirectorName")]
        [Column(TypeName = "varchar(500)")]
        public string DirectorName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string TypeOfCompany { get; set; }

        public DateTime CompanyStartDate { get; set; }
        public string Level { get; set; }
        public DateTime CompanyLicenceDate { get; set; }
        public DateTime CompanyRegisterDate { get; set; }
        public string EmailAddress { get; set; }
        public string GrnNo { get; set; }
        public string Address { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        [ForeignKey("State")]
        public int StateId { get; set; }
        public virtual State State { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City City { get; set; }

        public int PinCode { get; set; }
        public string TownName { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
