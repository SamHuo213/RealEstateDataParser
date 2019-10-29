using RealEstateDataParser.DataObjects;
using RealEstateDataParser.Maps;
using RealEstateDataParser.Services.UtilServices;
using SalesParser.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RealEstateDataParser.Services.ReportServices {
    public class SalByTypeReportService {
        private readonly BussinessDaysLookUpService bussinessDaysLookUpService;
        private readonly string totalType = "Total";

        public SalByTypeReportService() {
            bussinessDaysLookUpService = new BussinessDaysLookUpService();
        }

        public IEnumerable<SalByTypeReportEntry> GetSalByTypeReports(
            IEnumerable<PropertyTypeMixReportEntry> accumulatedSalesTypeMix,
            IEnumerable<PropertyTypeMixReportEntry> inventoryTypeMix,
            DateTime date
        ) {
            var types = GetTypes(accumulatedSalesTypeMix, inventoryTypeMix);
            var transformedAccumulatedSalesTypeMix = GetTransformedPropertyTypeMixReportEntries(accumulatedSalesTypeMix);
            var transformedInventoryTypeMix = GetTransformedPropertyTypeMixReportEntries(inventoryTypeMix);

            var salByTypeReports = new List<SalByTypeReportEntry>();
            foreach (var type in types) {
                AddSalByTypeReportEntry(
                    salByTypeReports,
                    transformedAccumulatedSalesTypeMix,
                    transformedInventoryTypeMix,
                    type,
                    date
                );
            }

            var salByTypes = salByTypeReports.OrderBy(x => x.PropertyType).ToList();
            AddSalByTypeReportEntry(
                salByTypes,
                transformedAccumulatedSalesTypeMix,
                transformedInventoryTypeMix,
                totalType,
                date
            );

            return salByTypes;
        }

        private IEnumerable<PropertyTypeMixReportEntry> GetTransformedPropertyTypeMixReportEntries(IEnumerable<PropertyTypeMixReportEntry> propertyTypeMixReportEntries) {
            return propertyTypeMixReportEntries.GroupBy(x => x.PropertyType = GetTransformedPropertyType(x.PropertyType))
                .Select(x => new PropertyTypeMixReportEntry() {
                    PropertyType = x.Key,
                    Count = x.Sum(c => c.Count)
                })
                .OrderBy(x => x.PropertyType);
        }

        private IEnumerable<string> GetTypes(IEnumerable<PropertyTypeMixReportEntry> accumulatedSalesTypeMix, IEnumerable<PropertyTypeMixReportEntry> inventoryTypeMix) {
            return accumulatedSalesTypeMix
                .Concat(inventoryTypeMix)
                .Select(x => GetTransformedPropertyType(x.PropertyType))
                .Distinct();
        }

        private void AddSalByTypeReportEntry(
            IList<SalByTypeReportEntry> salByTypeReports,
            IEnumerable<PropertyTypeMixReportEntry> accumulatedSalesTypeMix,
            IEnumerable<PropertyTypeMixReportEntry> inventoryTypeMix,
            string type,
            DateTime date
        ) {
            PropertyTypeMixReportEntry sale, inventory;
            if (type == totalType) {
                sale = accumulatedSalesTypeMix
                        .Aggregate((a, b) => new PropertyTypeMixReportEntry() {
                            PropertyType = totalType,
                            Count = a.Count + b.Count
                        });
                inventory = inventoryTypeMix
                        .Aggregate((a, b) => new PropertyTypeMixReportEntry() {
                            PropertyType = totalType,
                            Count = a.Count + b.Count
                        });
            } else {
                sale = accumulatedSalesTypeMix.FirstOrDefault(x => x.PropertyType == type);
                inventory = inventoryTypeMix.FirstOrDefault(x => x.PropertyType == type);
            }

            salByTypeReports.Add(GetSalByTypeReportEntry(sale, inventory, date));
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
            var monthlyBussinessDays = bussinessDaysLookUpService.GetNumberOfBusinessDaysInMonth(date.Year, date.Month);
            var holidays = bussinessDaysLookUpService.GetHolidaysInMonth(date.Year, date.Month);
            var bussinessDaysSoFar = BussinessDaysCalculatorService.GetBusinessDays(new DateTime(date.Year, date.Month, 1), date, holidays);
            var salesAsDouble = (double)sales;

            return (salesAsDouble / bussinessDaysSoFar) * monthlyBussinessDays;
        }

        private double GetSal(double projectedSales, int inventory) {
            if (inventory == 0) {
                return 0;
            }

            return projectedSales / inventory;
        }
    }
}
