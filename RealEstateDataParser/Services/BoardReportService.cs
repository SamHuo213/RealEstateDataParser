using SalesParser.DataObjects;
using SalesParser.Enums;
using System.Collections.Generic;
using System.Linq;

namespace SalesParser.Services {
    public class BoardReportService {
        private readonly CityReportService cityReportService;
        private readonly PropertyTypeMixReportService propertyTypeMixReportService;
        private readonly SalesDateReportService salesDateReportService;
        private readonly PricePointReportService pricePointReportService;

        public BoardReportService() {
            cityReportService = new CityReportService();
            propertyTypeMixReportService = new PropertyTypeMixReportService();
            salesDateReportService = new SalesDateReportService();
            pricePointReportService = new PricePointReportService();
        }

        public BoardReport GenerateReports(Board? board, IEnumerable<UnitEntry> soldUnitEntries, IEnumerable<UnitEntry> inventoryUnitEntries) {
            var filteredSoldUnitEntries = soldUnitEntries;
            var filteredInventoryUnitEntires = inventoryUnitEntries;
            if (board.HasValue) {
                filteredSoldUnitEntries = filteredSoldUnitEntries
                    .Where(x => x.Board == board);

                filteredInventoryUnitEntires = filteredInventoryUnitEntires
                    .Where(x => x.Board == board);
            }

            return new BoardReport() {
                CitySalesReports = cityReportService.GetCityReports(filteredSoldUnitEntries),
                SaleMixReports = propertyTypeMixReportService.GetPropertyTypeMixReports(filteredSoldUnitEntries),
                SaleDateReports = salesDateReportService.GetSalesDateReports(filteredSoldUnitEntries),
                SalesPricePointByCitiesReports = pricePointReportService.GetSoldPricePointByCityReports(filteredSoldUnitEntries),
                SalesPricePointByTypeReports = pricePointReportService.GetSoldPricePointByTypeReports(filteredSoldUnitEntries),
                SalesPricePointReports = pricePointReportService.GetSoldPricePointReports(filteredSoldUnitEntries),
                TotalSales = filteredSoldUnitEntries.Count(),
                InventoryCityReports = cityReportService.GetCityReports(filteredInventoryUnitEntires),
                InventoryMixReports = propertyTypeMixReportService.GetPropertyTypeMixReports(filteredInventoryUnitEntires),
                InventoryPricePointByCitiesReports = pricePointReportService.GetInventoryPricePointByCityReports(filteredInventoryUnitEntires),
                InventoryPricePointByTypeReports = pricePointReportService.GetInventoryPricePointByTypeReports(filteredInventoryUnitEntires),
                InventoryPricePointReports = pricePointReportService.GetInventoryPricePointReports(filteredInventoryUnitEntires),
                TotalInventory = filteredInventoryUnitEntires.Count()
            };
        }
    }
}
