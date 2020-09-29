using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UI.Models.Payments.Channels
{
    public class UPIChannel : PaymentChannelFormBase
    {
        [Required]
        [RegularExpression("^\\w+@\\w+$", ErrorMessage = "Please enter valid UPI ID")]
        public string VPA { get; set; }
    }
}