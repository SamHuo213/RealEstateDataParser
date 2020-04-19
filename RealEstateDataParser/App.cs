using RealEstateDataParser.DataObjects;
using RealEstateDataParser.Maps;
using SalesParser.DataObjects;
using SalesParser.Enums;
using SalesParser.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesParser {

    public class App {
        private readonly BoardReportService boardReportService;
        private readonly UnitFileService fileService;
        private readonly UnitEntryParserService unitEntryParserService;

        public App() {
            boardReportService = new BoardReportService();
            fileService = new UnitFileService();
            unitEntryParserService = new UnitEntryParserService();
        }

        public void Run(DateTime reportDate) {
            try {
                RunInternal(reportDate);
            } catch ( Exception e ) {
                var errorMessage = $"Failed message: {e.Message}";
                Console.WriteLine(errorMessage);
            }
        }

        private void RunInternal(DateTime reportDate) {
            var allLines = fileService.ReadAllLines(reportDate);
            var soldUnitEntriesDo = unitEntryParserService.ParseSoldUnitEntries(allLines, reportDate);
            var expiredUnitEntriesDo = unitEntryParserService.ParseExpiredEntries(allLines, reportDate);
            var monthlyAccumulatedsoldUnitEntriesDo = unitEntryParserService.ParseMontlyAccumulatedSoldUnitEntries(allLines, reportDate);
            var activeUnitEntriesDo = unitEntryParserService.ParseActiveUnitEntries(allLines);

            var boards = (Board[]) Enum.GetValues(typeof(Board));
            foreach ( var board in boards.Where(x => x != Board.Unknown) ) {
                var boardReport = boardReportService.GenerateReports(board, soldUnitEntriesDo, monthlyAccumulatedsoldUnitEntriesDo, activeUnitEntriesDo, reportDate);

                var reportLines = SalByTypeReportsToString(boardReport.SalByTypeReports);
                reportLines = InventoryCityReportsToString(boardReport.InventoryCityReports).Concat(reportLines);
                reportLines = InventoryMixReportsToString(boardReport.InventoryMixReports).Concat(reportLines);
                reportLines = InventoryPricePointReportsByCityToString(boardReport.InventoryPricePointByCitiesReports).Concat(reportLines);
                reportLines = InventoryPricePointReportsByTypeToString(boardReport.InventoryPricePointByTypeReports).Concat(reportLines);
                reportLines = InventoryPricePointReportsToString(boardReport.InventoryPricePointReports).Concat(reportLines);

                reportLines = MonthlyAccumulatedSalesCityReportsToString(boardReport.MonthlyAccumulatedCitySalesReports).Concat(reportLines);
                reportLines = MonthlyAccumulatedSalesMixReportsToString(boardReport.MonthlyAccumulatedSaleMixReports).Concat(reportLines);
                reportLines = MonthlyAccumulatedSalesDateReportsToString(boardReport.MonthlyAccumulatedSaleDateReports).Concat(reportLines);
                reportLines = MonthlyAccumulatedSalesPricePointReportsByCityToString(boardReport.MonthlyAccumulatedSalesPricePointByCitiesReports).Concat(reportLines);
                reportLines = MonthlyAccumulatedSalesPricePointReportsByTypeToString(boardReport.MonthlyAccumulatedSalesPricePointByTypeReports).Concat(reportLines);
                reportLines = MonthlyAccumulatedSalesPricePointReportsToString(boardReport.MonthlyAccumulatedSalesPricePointReports).Concat(reportLines);
                reportLines = MonthlyAccumulatedSalesOverUnderReportsToString(boardReport.MonthlyAccumulatedSalesOverUnderReports).Concat(reportLines);

                reportLines = SalesCityReportsToString(boardReport.CitySalesReports).Concat(reportLines);
                reportLines = SalesMixReportsToString(boardReport.SaleMixReports).Concat(reportLines);
                reportLines = SalesDateReportsToString(boardReport.SaleDateReports).Concat(reportLines);
                reportLines = SalesPricePointReportsByCityToString(boardReport.SalesPricePointByCitiesReports).Concat(reportLines);
                reportLines = SalesPricePointReportsByTypeToString(boardReport.SalesPricePointByTypeReports).Concat(reportLines);
                reportLines = SalesPricePointReportsToString(boardReport.SalesPricePointReports).Concat(reportLines);
                reportLines = SalesOverUnderReportsToString(boardReport.SalesOverUnderReports).Concat(reportLines);

                reportLines = MonthlyAccumulatedSalesToString(boardReport.MonthlyAccumulatedTotalSales).Concat(reportLines);
                reportLines = TotalSalesToString(boardReport.TotalSales).Concat(reportLines);
                reportLines = TotalInventoryToString(boardReport.TotalInventory).Concat(reportLines);

                fileService.WriteAllLines(reportDate, board.ToString(), reportLines);
            }
        }

        private IEnumerable<string> SalByTypeReportsToString(IEnumerable<SalByTypeReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Sal by Type...",
                "",
                "Property Type, Sales, Project Sales, Inventory, Sal, Sal (%)"
            };

            foreach ( var report in reports ) {
                var reportMessage = $"{report.PropertyType}, {report.Sales}, {report.ProjectedSales.ToString("0.##")}, {report.Inventory}, {report.Sal.ToString("0.###")}, {report.SalPercentage.ToString("0.##")}";
                lines.Add(reportMessage);
            }

            return lines;
        }

        private IEnumerable<string> SalesCityReportsToString(IEnumerable<CityReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Sales by City...",
                ""
            };

            return lines
                   .Concat(CityReportsToStringBoilerPlate(reports));
        }

        private IEnumerable<string> MonthlyAccumulatedSalesCityReportsToString(IEnumerable<CityReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Monthly Sales by City...",
                ""
            };

            return lines
                   .Concat(CityReportsToStringBoilerPlate(reports));
        }

        private IEnumerable<string> InventoryCityReportsToString(IEnumerable<CityReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Inventory by City...",
                ""
            };

            return lines
                   .Concat(CityReportsToStringBoilerPlate(reports));
        }

        private IEnumerable<string> CityReportsToStringBoilerPlate(IEnumerable<CityReportEntry> reports) {
            var lines = new List<string>();
            foreach ( var report in reports ) {
                var reportMessage = $"{report.City}, {report.SaleCount}";
                lines.Add(reportMessage);
            }

            return lines;
        }

        private IEnumerable<string> SalesMixReportsToString(IEnumerable<PropertyTypeMixReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Sales Mix...",
                ""
            };

            return lines
                .Concat(PropertyTypeMixReportsToStringBoilerPlate(reports));
        }

        private IEnumerable<string> MonthlyAccumulatedSalesMixReportsToString(IEnumerable<PropertyTypeMixReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Monthly Accumulated Sales Mix...",
                ""
            };

            return lines
                .Concat(PropertyTypeMixReportsToStringBoilerPlate(reports));
        }

        private IEnumerable<string> InventoryMixReportsToString(IEnumerable<PropertyTypeMixReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Inventory Mix...",
                ""
            };

            return lines
                .Concat(PropertyTypeMixReportsToStringBoilerPlate(reports));
        }

        private IEnumerable<string> PropertyTypeMixReportsToStringBoilerPlate(IEnumerable<PropertyTypeMixReportEntry> reports) {
            var lines = new List<string>();
            foreach ( var report in reports ) {
                var reportMessage = $"{report.PropertyType}, {report.Count}";
                lines.Add(reportMessage);
            }

            return lines;
        }

        private IEnumerable<string> SalesDateReportsToString(IEnumerable<SaleDateReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Exact Sales Date...",
                ""
            };

            foreach ( var report in reports ) {
                var reportMessage = $"{report.SaleDate}, {report.SaleCount}";
                lines.Add(reportMessage);
            }

            return lines;
        }

        private IEnumerable<string> MonthlyAccumulatedSalesDateReportsToString(IEnumerable<SaleDateReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Exact Monthly Accumulated Sales Date...",
                ""
            };

            foreach ( var report in reports ) {
                var reportMessage = $"{report.SaleDate}, {report.SaleCount}";
                lines.Add(reportMessage);
            }

            return lines;
        }

        private IEnumerable<string> SalesPricePointReportsByCityToString(IEnumerable<PricePointByKeyReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Sales Price Point By City...",
                ""
            };

            return lines
                .Concat(PricePointReportsByCityToStringBoilerPlate(reports));
        }

        private IEnumerable<string> SalesPricePointReportsByTypeToString(IEnumerable<PricePointByKeyReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Sales Price Point By Type...",
                ""
            };

            return lines
                .Concat(PricePointReportsByCityToStringBoilerPlate(reports));
        }

        private IEnumerable<string> MonthlyAccumulatedSalesPricePointReportsByCityToString(IEnumerable<PricePointByKeyReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Monthly Accumulated Sales Price Point By City...",
                ""
            };

            return lines
                .Concat(PricePointReportsByCityToStringBoilerPlate(reports));
        }

        private IEnumerable<string> MonthlyAccumulatedSalesPricePointReportsByTypeToString(IEnumerable<PricePointByKeyReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Monthly Accumulated Sales Price Point By Type...",
                ""
            };

            return lines
                .Concat(PricePointReportsByCityToStringBoilerPlate(reports));
        }

        private IEnumerable<string> InventoryPricePointReportsByCityToString(IEnumerable<PricePointByKeyReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Inventory Price Point By City...",
                ""
            };

            return lines
                .Concat(PricePointReportsByCityToStringBoilerPlate(reports));
        }

        private IEnumerable<string> InventoryPricePointReportsByTypeToString(IEnumerable<PricePointByKeyReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Inventory Price Point By Type...",
                ""
            };

            return lines
                .Concat(PricePointReportsByCityToStringBoilerPlate(reports));
        }

        private IEnumerable<string> PricePointReportsByCityToStringBoilerPlate(IEnumerable<PricePointByKeyReportEntry> reports) {
            var lines = new List<string>();
            foreach ( var cityReport in reports ) {
                lines.Add(cityReport.Key);
                lines.Add(" ");
                foreach ( var pricePointReport in cityReport.PricePointReports ) {
                    var reportMessage = $"{pricePointReport.PricePoint}, {pricePointReport.Count}";
                    lines.Add(reportMessage);
                }
                lines.Add(" ");
            }

            return lines;
        }

        private IEnumerable<string> SalesPricePointReportsToString(IEnumerable<PricePointReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Sales Price Point Report...",
                ""
            };

            return lines
                .Concat(PricePointReportsToStringBoilerPlate(reports));
        }

        private IEnumerable<string> MonthlyAccumulatedSalesPricePointReportsToString(IEnumerable<PricePointReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Monthly Accumulated Sales Price Point Report...",
                ""
            };

            return lines
                .Concat(PricePointReportsToStringBoilerPlate(reports));
        }

        private IEnumerable<string> InventoryPricePointReportsToString(IEnumerable<PricePointReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Inventory Price Point Report...",
                ""
            };

            return lines
                .Concat(PricePointReportsToStringBoilerPlate(reports));
        }

        private IEnumerable<string> PricePointReportsToStringBoilerPlate(IEnumerable<PricePointReportEntry> reports) {
            var lines = new List<string>();
            foreach ( var pricePointReport in reports ) {
                var reportMessage = $"{pricePointReport.PricePoint}, {pricePointReport.Count}";
                lines.Add(reportMessage);
            }

            return lines;
        }

        private IEnumerable<string> SalesOverUnderReportsToString(IEnumerable<OverUnderReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Sold Over Or Under Original Asking Price Report...",
                ""
            };

            return lines
                .Concat(SalesOverUnderReportsToStringBoilderPlate(reports));
        }

        private IEnumerable<string> MonthlyAccumulatedSalesOverUnderReportsToString(IEnumerable<OverUnderReportEntry> reports) {
            var lines = new List<string> {
                "",
                "...Monthly Accumulated Sales Over Or Under Original Asking Price Report...",
                ""
            };

            return lines
                .Concat(SalesOverUnderReportsToStringBoilderPlate(reports));
        }

        private IEnumerable<string> SalesOverUnderReportsToStringBoilderPlate(IEnumerable<OverUnderReportEntry> reports) {
            var lines = new List<string>();
            foreach ( var overUnderReport in reports ) {
                var reportMessage = $"{OverUnderEnumToStringMap.GetOverUnderString(overUnderReport.OverOrUnderAsking)}, {overUnderReport.Count}";
                lines.Add(reportMessage);
            }

            return lines;
        }

        private IEnumerable<string> TotalSalesToString(int totalSales) {
            return new List<string> {
                "",
                "...Total Sales...",
                $"{totalSales}"
            };
        }

        private IEnumerable<string> MonthlyAccumulatedSalesToString(int totalSales) {
            return new List<string> {
                "",
                "...Monthly Accumulated Sales...",
                $"{totalSales}"
            };
        }

        private IEnumerable<string> TotalInventoryToString(int totalInventory) {
            return new List<string> {
                "",
                "...Total Inventory...",
                $"{totalInventory}"
            };
        }
    }
}