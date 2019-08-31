using SalesParser.DataObjects;
using System.Collections.Generic;
using System.Linq;

namespace SalesParser.Services {

    public class CityReportService {

        public CityReportService() {
        }

        public IEnumerable<CityReportEntry> GetCityReports(IEnumerable<UnitEntry> UnitEntires) {
            var cityReports = UnitEntires
                .GroupBy(x => x.City)
                .Select(x => new CityReportEntry() {
                    City = x.Key,
                    SaleCount = x.Count()
                })
                .OrderBy(x => x.City);

            return cityReports;
        }
    }
}