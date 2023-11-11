using SV19T1081005.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV19T1081005.Web.Models
{
    public class ShipperPaginationResult : BasePaginationResult
    {
        public List<Shipper> Data { get; set; }
    }
}