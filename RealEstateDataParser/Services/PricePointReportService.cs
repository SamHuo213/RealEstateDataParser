﻿using SalesParser.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesParser.Services {
    public class PricePointReportService {
        public PricePointReportService() {
        }

        public IEnumerable<PricePointByCityReportEntry> GetSoldPricePointByCityReports(IEnumerable<UnitEntry> UnitEntires) {
            var groupedByCity = UnitEntires
                .GroupBy(x => x.City)
                .Select(x => new {
                    City = x.Key,
                    Units = x
                })
                .OrderBy(x => x.City);

            return groupedByCity
                .Select(x => new PricePointByCityReportEntry {
                    City = x.City,
                    PricePointReports = GetSoldPricePointReports(x.Units)
                });
        }

        public IEnumerable<PricePointReportEntry> GetSoldPricePointReports(IEnumerable<UnitEntry> UnitEntires) {
            return UnitEntires
                .GroupBy(x => Math.Floor(x.SoldPrice.Value / 100000) * 100000)
                .OrderBy(x => x.Key)
                .Select(x => new PricePointReportEntry() {
                    PricePoint = PricePointKeyFormate(x.Key, x.Key + 100000),
                    Count = x.Count()
                });
        }

        public IEnumerable<PricePointByCityReportEntry> GetInventoryPricePointByCityReports(IEnumerable<UnitEntry> UnitEntires) {
            var groupedByCity = UnitEntires
                .GroupBy(x => x.City)
                .Select(x => new {
                    City = x.Key,
                    Units = x
                })
                .OrderBy(x => x.City);

            return groupedByCity
                .Select(x => new PricePointByCityReportEntry {
                    City = x.City,
                    PricePointReports = GetInventoryPricePointReports(x.Units)
                });
        }

        public IEnumerable<PricePointReportEntry> GetInventoryPricePointReports(IEnumerable<UnitEntry> UnitEntires) {
            return UnitEntires
                .GroupBy(x => Math.Floor(x.FinalAskingPrice / 100000) * 100000)
                .OrderBy(x => x.Key)
                .Select(x => new PricePointReportEntry() {
                    PricePoint = PricePointKeyFormate(x.Key, x.Key + 100000),
                    Count = x.Count()
                });
        }

        private string PricePointKeyFormate(double rangeStart, double rangeEnd) {
            return $"{string.Format("{0:n0}", rangeStart)} - {string.Format("{0:n0}", rangeEnd)}";
        }
    }
}