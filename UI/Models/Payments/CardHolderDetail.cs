using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UI.Models.Payments
{
    public class CardHolderDetail
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^\d{12,19}$", ErrorMessage = "Please enter valid card no.")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Required")]
        public string ExpiryMonth { get; set; }

        [Required(ErrorMessage = "Required")]
        public string ExpiryDate { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(3)]
        public string CVV { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(4)]
        public string Pin { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(22, ErrorMessage = "Please enter valid name")]
        public string Name { get; set; }

        public Dictionary<string, string> Months
        {
            get
            {
                Dictionary<string, string> expirationDateMonths = new Dictionary<string, string>();

                for (int i = 1; i <= 12; i++)
                {
                    DateTime month = new DateTime(2000, i, 1);
                    expirationDateMonths.Add(month.ToString("MM"), month.ToString("MMM"));
                }

                return expirationDateMonths;
            }
        }

        public Dictionary<string, string> Years
        {
            get
            {
                //Create the credit card expiration year SelectList (go out 12 years) 
                Dictionary<string, string> expirationDateYears = new Dictionary<string, string>();
                for (int i = 0; i <= 11; i++)
                {
                    String year = (DateTime.Today.Year + i).ToString();
                    expirationDateYears.Add(year, year);
                }

                return expirationDateYears;
            }
        }
    }
}