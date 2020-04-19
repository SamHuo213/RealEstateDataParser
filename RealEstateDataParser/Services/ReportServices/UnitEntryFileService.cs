using RealEstateDataParser.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace SalesParser.Services {

    public class UnitFileService {

        public UnitFileService() {
        }

        public IEnumerable<string> ReadAllLines(DateTime dateTime) {
            var DateString = dateTime.Date.ToString("yyyy-MM-dd");
            var DateFile = $"{DateString}.txt";
            var filePath = Path.Combine(ConfigurationService.Configuration["dataFilePath"], DateString, DateFile);
            return File.ReadAllLines(filePath);
        }

        public void WriteAllLines(DateTime dateTime, string prefix, IEnumerable<string> lines) {
            var DateString = dateTime.Date.ToString("yyyy-MM-dd");
            var DateFile = string.IsNullOrEmpty(prefix)
                ? $"{DateString}_report.txt"
                : $"{prefix}_{DateString}_report.txt";

            var filePath = Path.Combine(ConfigurationService.Configuration["dataFilePath"], DateString, DateFile);

            File.WriteAllLines(filePath, lines);
        }
    }
}