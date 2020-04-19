using RealEstateDataParser.Enums;

namespace RealEstateDataParser.DataObjects {

    public class OverUnderReportEntry {
        public OverUnder OverOrUnderAsking { get; set; }

        public int Count { get; set; }
    }
}