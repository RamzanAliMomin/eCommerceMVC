using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UI.Models.Payments.Channels;

namespace UI.Models.Payments
{
    public class PaymentPage
    {
        [Required]
        public int ApplicationID { get; set; }

        [Required]
        public OrderInfo OrderInfo { get; set; }

        public UserInfo UserInfo { get; set; }

        [Required]
        public string PaymentMode { get; set; }

        public Dictionary<string, string> Channels { get; set; }

        public CreditCardChannel CreditCardInfo { get; set; }

        public DebitCardChannel DebitCardInfo { get; set; }

        public UPIChannel UPIInfo { get; set; }

        public NetbankingChannel NetbankingInfo { get; set; }

        public PaytmChannel PaytmInfo { get; set; }
    }
}