using System.Collections.Generic;

namespace SalesParser.DataObjects {

    public class PricePointByCityReportEntry {
        public string City { get; set; }

        public IEnumerable<PricePointReportEntry> PricePointReports { get; set; }
    }
}