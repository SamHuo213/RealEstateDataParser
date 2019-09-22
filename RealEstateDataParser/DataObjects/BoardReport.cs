using RealEstateDataParser.DataObjects;
using System.Collections.Generic;

namespace SalesParser.DataObjects {
    public class BoardReport {
        public IEnumerable<CityReportEntry> CitySalesReports { get; set; }

        public IEnumerable<SaleDateReportEntry> SaleDateReports { get; set; }

        public IEnumerable<PropertyTypeMixReportEntry> SaleMixReports { get; set; }

        public IEnumerable<PricePointByKeyReportEntry> SalesPricePointByCitiesReports { get; set; }

        public IEnumerable<PricePointByKeyReportEntry> SalesPricePointByTypeReports { get; set; }

        public IEnumerable<PricePointReportEntry> SalesPricePointReports { get; set; }

        public int TotalSales { get; set; }

        public IEnumerable<CityReportEntry> MonthlyAccumulatedCitySalesReports { get; set; }

        public IEnumerable<SaleDateReportEntry> MonthlyAccumulatedSaleDateReports { get; set; }

        public IEnumerable<PropertyTypeMixReportEntry> MonthlyAccumulatedSaleMixReports { get; set; }

        public IEnumerable<PricePointByKeyReportEntry> MonthlyAccumulatedSalesPricePointByCitiesReports { get; set; }

        public IEnumerable<PricePointByKeyReportEntry> MonthlyAccumulatedSalesPricePointByTypeReports { get; set; }

        public IEnumerable<PricePointReportEntry> MonthlyAccumulatedSalesPricePointReports { get; set; }

        public int MonthlyAccumulatedTotalSales { get; set; }

        public IEnumerable<CityReportEntry> InventoryCityReports { get; set; }

        public IEnumerable<PropertyTypeMixReportEntry> InventoryMixReports { get; set; }

        public IEnumerable<PricePointByKeyReportEntry> InventoryPricePointByCitiesReports { get; set; }

        public IEnumerable<PricePointByKeyReportEntry> InventoryPricePointByTypeReports { get; set; }

        public IEnumerable<PricePointReportEntry> InventoryPricePointReports { get; set; }

        public int TotalInventory { get; set; }

        public IEnumerable<SalByTypeReportEntry> SalByTypeReports { get; set; }
    }
}
