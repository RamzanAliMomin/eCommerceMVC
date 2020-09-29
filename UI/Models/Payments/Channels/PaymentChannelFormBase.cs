using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Payments.Channels
{
    public abstract class PaymentChannelFormBase
    {
        public string PaymentMode { get; set; }

        public string DisplayName { get; set; }
    }
}