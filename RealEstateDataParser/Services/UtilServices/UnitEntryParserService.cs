using SalesParser.DataObjects;
using SalesParser.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SalesParser.Services {
    public class UnitEntryParserService {
        private const string UpdatedString = "updated";
        private const int EntryPropertyCount = 138;

        private delegate bool FilterPredicate(UnitEntry unitEntry);

        public UnitEntryParserService() {
        }

        public IEnumerable<UnitEntry> ParseSoldUnitEntries(IEnumerable<string> rawEntries, DateTime? filterDate = null) {
            return ParseUnitEntriesBoilerPlate(
                rawEntries,
                parseSold: true,
                unitEntry => filterDate?.Date == unitEntry.ReportDate
                );
        }

        public IEnumerable<UnitEntry> ParseMontlyAccumulatedSoldUnitEntries(IEnumerable<string> rawEntries, DateTime? filterDate = null) {
            return ParseUnitEntriesBoilerPlate(
                rawEntries,
                parseSold: true,
                unitEntry =>
                    filterDate?.Date.Month == unitEntry.ReportDate?.Month &&
                    filterDate?.Date.Year == unitEntry.ReportDate?.Year
                );
        }

        public IEnumerable<UnitEntry> ParseActiveUnitEntries(IEnumerable<string> rawEntries) {
            return ParseUnitEntriesBoilerPlate(
                rawEntries,
                parseSold: false,
                unitEntry => true
                );
        }

        private IEnumerable<UnitEntry> ParseUnitEntriesBoilerPlate(IEnumerable<string> rawEntries, bool parseSold, FilterPredicate filterPredicate) {
            var processedEntries = new List<UnitEntry>();
            foreach (var entry in rawEntries) {
                if (string.IsNullOrEmpty(entry)) {
                    continue;
                }

                var firstWord = entry.Substring(0, 7).ToLower();
                if (firstWord == UpdatedString) {
                    continue;
                }

                var unitEntry = ParseUnitEntry(entry, parseSold);
                if (unitEntry == null) {
                    continue;
                }

                if (!filterPredicate(unitEntry)) {
                    continue;
                }

                processedEntries.Add(unitEntry);
            }

            return processedEntries;
        }

        private UnitEntry ParseUnitEntry(string rawEntry, bool parseSold) {
            var entryArray = rawEntry.Split("\t");
            if (entryArray.Length < EntryPropertyCount) {
                return null;
            }

            var soldDateString = entryArray[33];
            if (parseSold && string.IsNullOrEmpty(soldDateString)) {
                return null;
            } else if (!parseSold && !string.IsNullOrEmpty(soldDateString)) {
                return null;
            }

            return new UnitEntry() {
                MlsId = entryArray[0],
                Address = entryArray[5],
                Type = entryArray[9],
                SoldPrice = GetSoldPrice(entryArray[32]),
                FinalAskingPrice = Double.Parse(entryArray[7]) * 1000,
                OriginalAskingPrice = Double.Parse(entryArray[28]) * 1000,
                SoldDate = GetDate(entryArray[33]),
                ReportDate = GetDate(entryArray[34]),
                City = entryArray[134],
                OwnershipInterest = entryArray[136],
                Board = GetBoardFromRaw(entryArray[124]),
                RawData = rawEntry
            };
        }

        private double? GetSoldPrice(string soldPrice) {
            if (string.IsNullOrEmpty(soldPrice)) {
                return null;
            }

            return Double.Parse(soldPrice) * 1000;
        }

        private Board GetBoardFromRaw(string boardString) {
            if (boardString.ToLower() == "v") {
                return Board.GreaterVancouver;
            } else if (boardString.ToLower() == "f") {
                return Board.FraserVally;
            } else if (boardString.ToLower() == "h") {
                return Board.Chilliwack;
            }

            return Board.Unknown;
        }

        private DateTime? GetDate(string dateTimeString) {
            if (string.IsNullOrEmpty(dateTimeString)) {
                return null;
            }

            return DateTime.Parse(
                dateTimeString,
                new CultureInfo("en-US")
            );
        }
    }
}
