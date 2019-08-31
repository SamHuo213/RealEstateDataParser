using SalesParser.DataObjects;
using System.Collections.Generic;
using System.Linq;

namespace SalesParser.Services {
    public class PropertyTypeMixReportService {
        public PropertyTypeMixReportService() {
        }

        public IEnumerable<PropertyTypeMixReportEntry> GetPropertyTypeMixReports(IEnumerable<UnitEntry> UnitEntires) {
            var propertyTypeMixReport = UnitEntires
                .GroupBy(x => x.Type)
                .Select(x => new PropertyTypeMixReportEntry() {
                    PropertyType = x.Key,
                    Count = x.Count()
                })
                .OrderBy(x => x.PropertyType);

            return propertyTypeMixReport;
        }
    }
}
