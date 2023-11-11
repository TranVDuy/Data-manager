using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081005.DomainModel
{
    /// <summary>
    /// Mặt hàng
    /// </summary>
    public class Product
    {
        public int ProductID { set; get; }
        public string ProductName { get; set; }
        public int SupplierID { get; set; }
        public int CategoryID { get; set; }
        public string Unit { get; set; }
        public string Price { get; set; }
        public string Photo { get; set; }
    }
}
