using SalesParser.DataObjects;
using System.Collections.Generic;
using System.Linq;

namespace SalesParser.Services {

    public class SalesDateReportService {

        public SalesDateReportService() {
        }

        public IEnumerable<SaleDateReportEntry> GetSalesDateReports(IEnumerable<UnitEntry> UnitEntires) {
            var saleDateReports = UnitEntires
                .GroupBy(x => x.SoldDate.Value.ToString("yyyy-MM-dd"))
                .Select(x => new SaleDateReportEntry() {
                    SaleDate = x.Key,
                    SaleCount = x.Count()
                })
                .OrderBy(x => x.SaleDate);

            return saleDateReports;
        }
    }
}