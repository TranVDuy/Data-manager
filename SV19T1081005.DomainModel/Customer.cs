using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081005.DomainModel
{
    /// <summary>
    /// Khách hàng
    /// </summary>
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerName { set; get; }
        public string ContactName { set; get; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        /// <summary>
        /// Tên quốc gia của khách hàng
        /// </summary>
        public string Country { get; set; }
       
    }
}
