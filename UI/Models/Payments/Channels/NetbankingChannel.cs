using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UI.Models.Payments.Channels
{
    public class NetbankingChannel : PaymentChannelFormBase
    {
        [Required(ErrorMessage = "Select your bank")]
        public string SelectedPayChannel { get; set; }

        public Dictionary<string, string> PayChannelOptions { get; set; }
    }
}