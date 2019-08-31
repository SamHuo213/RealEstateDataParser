using System.Collections.Generic;

namespace SalesParser.DataObjects {
    public class PricePointByKeyReportEntry {
        public string Key { get; set; }

        public IEnumerable<PricePointReportEntry> PricePointReports { get; set; }
    }
}
