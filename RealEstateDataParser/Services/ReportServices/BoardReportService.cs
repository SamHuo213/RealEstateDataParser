using RealEstateDataParser.Enums;
using RealEstateDataParser.Services.ReportServices;
using SalesParser.DataObjects;
using SalesParser.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesParser.Services {

    public class BoardReportService {
        private readonly CityReportService cityReportService;
        private readonly PropertyTypeMixReportService propertyTypeMixReportService;
        private readonly SalesDateReportService salesDateReportService;
        private readonly PricePointReportService pricePointReportService;
        private readonly SalByTypeReportService salByTypeReportService;
        private readonly OverUnderReportService overUnderReportService;

        public BoardReportService() {
            cityReportService = new CityReportService();
            propertyTypeMixReportService = new PropertyTypeMixReportService();
            salesDateReportService = new SalesDateReportService();
            pricePointReportService = new PricePointReportService();
            salByTypeReportService = new SalByTypeReportService();
            overUnderReportService = new OverUnderReportService();
        }

        public BoardReport GenerateReports(
                Board? board,
                IEnumerable<UnitEntry> soldUnitEntries,
                IEnumerable<UnitEntry> monthlyAccumulatedSoldUnitEntries,
                IEnumerable<UnitEntry> monthlyAccumulatedExpiredUnitEntriesDo,
                IEnumerable<UnitEntry> expiredUnitEntriesDo,
                IEnumerable<UnitEntry> inventoryUnitEntries,
                DateTime reportDate
        ) {
            var filteredSoldUnitEntries = soldUnitEntries;
            var filteredMonthlyAccumulatedSoldUnitEntries = monthlyAccumulatedSoldUnitEntries;
            var filteredMonthlyAccumulatedExpiredUnitEntriesDo = monthlyAccumulatedExpiredUnitEntriesDo;
            var filteredExpiredUnitEntriesDo = expiredUnitEntriesDo;
            var filteredInventoryUnitEntires = inventoryUnitEntries;

            if ( board.HasValue ) {
                filteredSoldUnitEntries = filteredSoldUnitEntries
                    .Where(x => x.Board == board);

                filteredMonthlyAccumulatedSoldUnitEntries = filteredMonthlyAccumulatedSoldUnitEntries
                    .Where(x => x.Board == board);

                filteredMonthlyAccumulatedExpiredUnitEntriesDo = filteredMonthlyAccumulatedExpiredUnitEntriesDo
                    .Where(x => x.Board == board);

                filteredExpiredUnitEntriesDo = filteredExpiredUnitEntriesDo
                    .Where(x => x.Board == board);

                filteredInventoryUnitEntires = filteredInventoryUnitEntires
                    .Where(x => x.Board == board);
            }

            var boardReport = new BoardReport() {
                CitySalesReports = cityReportService.GetCityReports(filteredSoldUnitEntries),
                SaleMixReports = propertyTypeMixReportService.GetPropertyTypeMixReports(filteredSoldUnitEntries),
                SaleDateReports = salesDateReportService.GetSalesDateReports(filteredSoldUnitEntries),
                SalesPricePointByCitiesReports = pricePointReportService.GetSoldPricePointByCityReports(filteredSoldUnitEntries),
                SalesPricePointByTypeReports = pricePointReportService.GetSoldPricePointByTypeReports(filteredSoldUnitEntries),
                SalesPricePointReports = pricePointReportService.GetSoldPricePointReports(filteredSoldUnitEntries),
                SalesOverUnderReports = overUnderReportService.GetOverUnderReports(filteredSoldUnitEntries),
                TotalSales = filteredSoldUnitEntries.Count(),

                MonthlyAccumulatedCitySalesReports = cityReportService.GetCityReports(filteredMonthlyAccumulatedSoldUnitEntries),
                MonthlyAccumulatedSaleDateReports = salesDateReportService.GetSalesDateReports(filteredMonthlyAccumulatedSoldUnitEntries),
                MonthlyAccumulatedSaleMixReports = propertyTypeMixReportService.GetPropertyTypeMixReports(filteredMonthlyAccumulatedSoldUnitEntries),
                MonthlyAccumulatedSalesPricePointByCitiesReports = pricePointReportService.GetSoldPricePointByCityReports(filteredMonthlyAccumulatedSoldUnitEntries),
                MonthlyAccumulatedSalesPricePointByTypeReports = pricePointReportService.GetSoldPricePointByTypeReports(filteredMonthlyAccumulatedSoldUnitEntries),
                MonthlyAccumulatedSalesPricePointReports = pricePointReportService.GetSoldPricePointReports(filteredMonthlyAccumulatedSoldUnitEntries),
                MonthlyAccumulatedSalesOverUnderReports = overUnderReportService.GetOverUnderReports(filteredMonthlyAccumulatedSoldUnitEntries),
                MonthlyAccumulatedTotalSales = filteredMonthlyAccumulatedSoldUnitEntries.Count(),

                MonthlyAccumulatedTotalCancelProtected = filteredMonthlyAccumulatedExpiredUnitEntriesDo.Count(x => x.ListingStatus == ListingStatus.cancelProtected),
                MonthlyAccumulatedTotalTerminated = filteredMonthlyAccumulatedExpiredUnitEntriesDo.Count(x => x.ListingStatus == ListingStatus.terminated),
                MonthlyAccumulatedTotalExpired = filteredMonthlyAccumulatedExpiredUnitEntriesDo.Count(x => x.ListingStatus == ListingStatus.expired),

                TotalCancelProtected = filteredExpiredUnitEntriesDo.Count(x => x.ListingStatus == ListingStatus.cancelProtected),
                TotalTerminated = filteredExpiredUnitEntriesDo.Count(x => x.ListingStatus == ListingStatus.terminated),
                TotalExpired = filteredExpiredUnitEntriesDo.Count(x => x.ListingStatus == ListingStatus.expired),

                InventoryCityReports = cityReportService.GetCityReports(filteredInventoryUnitEntires),
                InventoryMixReports = propertyTypeMixReportService.GetPropertyTypeMixReports(filteredInventoryUnitEntires),
                InventoryPricePointByCitiesReports = pricePointReportService.GetInventoryPricePointByCityReports(filteredInventoryUnitEntires),
                InventoryPricePointByTypeReports = pricePointReportService.GetInventoryPricePointByTypeReports(filteredInventoryUnitEntires),
                InventoryPricePointReports = pricePointReportService.GetInventoryPricePointReports(filteredInventoryUnitEntires),
                TotalInventory = filteredInventoryUnitEntires.Count()
            };

            boardReport.SalByTypeReports = salByTypeReportService.GetSalByTypeReports(boardReport.MonthlyAccumulatedSaleMixReports, boardReport.InventoryMixReports, reportDate);

            return boardReport;
        }
    }
}