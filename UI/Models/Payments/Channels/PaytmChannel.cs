using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UI.Models.Payments.Channels
{
    public class PaytmChannel : PaymentChannelFormBase
    {
        public bool IsAuthorized { get; set; }

        public double? PaytmBalance { get; set; }

        public string Currency { get; set; }

        [Required]
        [RegularExpression(@"^[6789]\d{9}$", ErrorMessage = "Please enter valid MobileNo.")]
        [MaxLength(10, ErrorMessage = "Mobile must be 10 digit")]
        public string MobileNo { get; set; }

        public PaytmLogin Login { get; set; }
    }

    public class PaytmLogin
    {
        [Required]
        public string OTP { get; set; }
    }
}