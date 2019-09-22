using SalesParser.Enums;
using System;

namespace SalesParser.DataObjects {
    public class UnitEntry {
        public string MlsId { get; set; }

        public string Address { get; set; }

        public string Type { get; set; }

        public double? SoldPrice { get; set; }

        public double FinalAskingPrice { get; set; }

        public double OriginalAskingPrice { get; set; }

        public DateTime? SoldDate { get; set; }

        public DateTime? ReportDate { get; set; }

        public string City { get; set; }

        public string OwnershipInterest { get; set; }

        public Board Board { get; set; }

        public string RawData { get; set; }
    }
}
