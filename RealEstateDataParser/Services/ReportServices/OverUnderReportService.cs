using RealEstateDataParser.DataObjects;
using RealEstateDataParser.Enums;
using SalesParser.DataObjects;
using System.Collections.Generic;
using System.Linq;

namespace RealEstateDataParser.Services.ReportServices {
    public class OverUnderReportService {
        public OverUnderReportService() {
        }

        public IEnumerable<OverUnderReportEntry> GetOverUnderReports(IEnumerable<UnitEntry> UnitEntires) {
            var overUnderReports = UnitEntires
                .GroupBy(x => GetOverOrUnderKey(x.SoldPrice.Value, x.OriginalAskingPrice))
                .Select(x => new OverUnderReportEntry() {
                    OverOrUnderAsking = x.Key,
                    Count = x.Count()
                })
                .OrderBy(x => x.OverOrUnderAsking);

            return overUnderReports;
        }

        private OverUnder GetOverOrUnderKey(double soldPrice, double originalAskingPrice) {
            if (soldPrice > originalAskingPrice) {
                return OverUnder.over;
            } else if (soldPrice < originalAskingPrice) {
                return OverUnder.under;
            }

            return OverUnder.soldAt;
        }
    }
}
