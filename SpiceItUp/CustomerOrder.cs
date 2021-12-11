using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    public class CustomerOrder
    {
        protected string storeLocation;
        protected int orderId;
        private readonly DateTime date = DateTime.Now;
        protected string[,] customerCart = { { "Product", "Quantity", "Price" } };
    }
}
