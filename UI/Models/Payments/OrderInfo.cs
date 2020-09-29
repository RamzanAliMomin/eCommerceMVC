using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UI.Models.Payments
{
    public class OrderInfo
    {
        public string OrderID { get; set; }

        public string TransactionToken { get; set; }

        public List<OrderItem> Items { get; set; }

        public double TotalAmount
        {
            get
            {
                if (Items == null || !Items.Any())
                    return 0;

                return Items.DefaultIfEmpty().Sum(x => x.Amount);
            }
        }
    }

    public class OrderItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Amount { get; set; }
    }
}