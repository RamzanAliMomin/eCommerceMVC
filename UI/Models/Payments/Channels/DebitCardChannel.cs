using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Payments.Channels
{
    public class DebitCardChannel : PaymentChannelFormBase
    {
        public CardHolderDetail CardHolderDetail { get; set; }
    }
}