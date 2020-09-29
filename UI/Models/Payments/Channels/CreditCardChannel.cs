using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.Models.Payments;

namespace UI.Models.Payments.Channels
{
    public class CreditCardChannel : PaymentChannelFormBase
    {
        public CardHolderDetail CardHolderDetail { get; set; }
    }
}