using RealEstateDataParser.DataObjects;
using RealEstateDataParser.Maps;
using SalesParser.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RealEstateDataParser.Services.ReportServices {
    public class SalByTypeReportService {
        private readonly BussinessDaysService bussinessDaysService;

        public SalByTypeReportService() {
            bussinessDaysService = new BussinessDaysService();
        }

        public IEnumerable<SalByTypeReportEntry> GetSalByTypeReports(
            IEnumerable<PropertyTypeMixReportEntry> accumulatedSalesTypeMix,
            IEnumerable<PropertyTypeMixReportEntry> inventoryTypeMix,
            DateTime date
        ) {
            var types = accumulatedSalesTypeMix
                .Concat(inventoryTypeMix)
                .Select(x => GetTransformedPropertyType(x.PropertyType))
                .Distinct();

            var transformedAccumulatedSalesTypeMix = accumulatedSalesTypeMix
                .GroupBy(x => x.PropertyType = GetTransformedPropertyType(x.PropertyType))
                .Select(x => new PropertyTypeMixReportEntry() {
                    PropertyType = x.Key,
                    Count = x.Sum(c => c.Count)
                })
                .OrderBy(x => x.PropertyType);

            var transformedInventoryTypeMix = inventoryTypeMix
                .GroupBy(x => x.PropertyType = GetTransformedPropertyType(x.PropertyType))
                .Select(x => new PropertyTypeMixReportEntry() {
                    PropertyType = x.Key,
                    Count = x.Sum(c => c.Count)
                })
                .OrderBy(x => x.PropertyType);

            var salByTypeReports = new List<SalByTypeReportEntry>();
            foreach (var type in types) {
                var sale = transformedAccumulatedSalesTypeMix.FirstOrDefault(x => x.PropertyType == type);
                var inventory = transformedInventoryTypeMix.FirstOrDefault(x => x.PropertyType == type);

                salByTypeReports.Add(GetSalByTypeReportEntry(sale, inventory, date));
            }

            return salByTypeReports
                .OrderBy(x => x.PropertyType);
        }

        private string GetTransformedPropertyType(string propertyType) {
            return PropertyTypeKeyMap.GetSalPropertyTypeKey(propertyType);
        }

        private SalByTypeReportEntry GetSalByTypeReportEntry(PropertyTypeMixReportEntry sales, PropertyTypeMixReportEntry inventory, DateTime date) {
            var newSalByTypeReportEntry = new SalByTypeReportEntry();
            if (sales == null) {
                newSalByTypeReportEntry.PropertyType = inventory.PropertyType;
                newSalByTypeReportEntry.Sales = 0;
                newSalByTypeReportEntry.ProjectedSales = 0;
            } else {
                newSalByTypeReportEntry.PropertyType = sales.PropertyType;
                newSalByTypeReportEntry.Sales = sales.Count;
                newSalByTypeReportEntry.ProjectedSales = GetProjectedSales(sales.Count, date);
            }

            if (inventory == null) {
                newSalByTypeReportEntry.Inventory = 0;
            } else {
                newSalByTypeReportEntry.Inventory = inventory.Count;
            }

            newSalByTypeReportEntry.Sal = GetSal(newSalByTypeReportEntry.ProjectedSales, newSalByTypeReportEntry.Inventory);
            newSalByTypeReportEntry.SalPercentage = newSalByTypeReportEntry.Sal * 100;

            return newSalByTypeReportEntry;
        }

        private double GetProjectedSales(int sales, DateTime date) {
            var monthlyBussinessDays = bussinessDaysService.GetNumberOfBusinessDaysInMonth(date.Year, date.Month);
            var holidays = bussinessDaysService.GetHolidaysInMonth(date.Year, date.Month);
            var bussinessDaysSoFar = GetBusinessDays(new DateTime(date.Year, date.Month, 1), date, holidays);
            var salesAsDouble = (double)sales;

            return (salesAsDouble / bussinessDaysSoFar) * monthlyBussinessDays;
        }

        private double GetSal(double projectedSales, int inventory) {
            if (inventory == 0) {
                return 0;
            }

            return projectedSales / inventory;
        }

        private static int GetBusinessDays(DateTime start, DateTime end, IEnumerable<int> holidays) {
            DateTime current = start;
            DateTime inclusiveEnd = end;

            var bussinessDayCount = 0;
            while (current <= inclusiveEnd) {
                if (
                    current.DayOfWeek != DayOfWeek.Saturday
                    && current.DayOfWeek != DayOfWeek.Sunday
                    && !holidays.Contains(current.Day)
                ) {
                    bussinessDayCount++;
                }

                current = current.AddDays(1);
            }

            return bussinessDayCount;
        }
    }
}
